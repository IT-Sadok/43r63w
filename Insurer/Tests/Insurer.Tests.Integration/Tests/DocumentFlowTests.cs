using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Company.Application.Models.Responses;
using Company.Infrastructure.Data;
using FluentAssertions;
using Insurer.Tests.Integration.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Minio.DataModel.Args;

namespace Insurer.Tests.Integration.Tests;

public class DocumentFlowTests :
    IClassFixture<IntegrationTestFixture>,
    IClassFixture<TestUserContext>
{
    private readonly TestUserContext _userContextAccessor;
    private readonly IntegrationTestFixture _integrationTestFixture;

    private const string ObjectKey = "test\\test.png";
    private const string FilePath = "C:\\Users\\Vlad\\Desktop\\test.png";

    public DocumentFlowTests(
        IntegrationTestFixture integrationTestFixture,
        TestUserContext userContextAccessor)
    {
        _integrationTestFixture = integrationTestFixture;
        _userContextAccessor = userContextAccessor;
    }

    [Fact]
    public async Task Minio_Should_Upload_File()
    {
        await using var file = File.OpenRead(FilePath);

        var response = await _integrationTestFixture.Minio.MinioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(MinioFixture.BucketName)
                .WithObject("test")
                .WithStreamData(file)
                .WithObjectSize(file.Length)
                .WithContentType("image/png"));

        response.ResponseStatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Minio_Should_Upload_PressignUrl_And_Upload_Content()
    {
        await UploadDocument();

        var url = await _integrationTestFixture.Minio.MinioClient.PresignedGetObjectAsync(
            new PresignedGetObjectArgs()
                .WithBucket(MinioFixture.BucketName)
                .WithObject(ObjectKey)
                .WithExpiry(3600));

        url.Should().NotBeNullOrEmpty();

        using var httpClient = new HttpClient();

        var file = await File.ReadAllBytesAsync(FilePath);

        var result = await httpClient.GetAsync(url);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var objectStat = await _integrationTestFixture.Minio.MinioClient.StatObjectAsync(
            new StatObjectArgs()
                .WithBucket(MinioFixture.BucketName)
                .WithObject(ObjectKey));

        objectStat.Should().NotBeNull();
        objectStat.Size.Should().BeLessThanOrEqualTo(file.Length);
    }

    [Fact]
    public async Task UploadDocument_Full_Workflow_Should_Return_Ok()
    {
        await _integrationTestFixture.Factory.ResetDbAsync();

        await using var scope = _integrationTestFixture.Factory.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CompanyDbContext>();

        var company = new Company.Domain.Entity.Company
        {
            CompanyName = "test-company",
            RegistrationNumber = 3487587158,
            Phone = "38143827574",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };
        
            dbContext.Companies.Add(company);
            await dbContext.SaveChangesAsync();
        


        var httpClient = _integrationTestFixture.Factory.CreateDefaultClient();
        var token = JwtGenerator.GenerateToken();
        var fileBytes = await File.ReadAllBytesAsync(FilePath);

        var fileContent = new ByteArrayContent(fileBytes);
        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "\"file\"",
            FileName = "\"test.png\""
        };
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

        var form = new MultipartFormDataContent();
        form.Add(fileContent, "file", "test.png");

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var responseMessage = await httpClient.PostAsync("/documents", form);

        if (!responseMessage.IsSuccessStatusCode)
        {
            var error = await responseMessage.Content.ReadAsStringAsync();
            throw new Exception("api error: " + error);
        }

        var responseContent = await responseMessage.Content.ReadFromJsonAsync<CreateDocumentResponse>();
        
        responseContent.Should().NotBeNull();
        responseContent.Success.Should().BeTrue();
    }

    private async Task UploadDocument()
    {
        await using var file = File.OpenRead(FilePath);

        var response = await _integrationTestFixture.Minio.MinioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(MinioFixture.BucketName)
                .WithObject(ObjectKey)
                .WithStreamData(file)
                .WithObjectSize(file.Length)
                .WithContentType("image/png"));
    }
}