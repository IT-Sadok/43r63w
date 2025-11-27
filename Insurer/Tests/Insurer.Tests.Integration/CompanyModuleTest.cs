using System.Net;
using System.Net.Http.Json;
using Company.Application.Models.Request;
using Company.Application.Models.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Insurer.Tests.Integration;

public class CompanyModuleTest :
    IClassFixture<CompanyFactory>,
    IAsyncLifetime
{
    private readonly CompanyFactory _companyFactory;

    public CompanyModuleTest(CompanyFactory factory)
    {
        _companyFactory = factory;
    }

    [Fact]
    public async Task CreateCompany_Should_Give200Response()
    {
        await _companyFactory.ResetDbAsync();

        var client = _companyFactory.CreateClient();

        var token = JwtGenerator.GenerateToken();

        var company = new CreateCompanyRequest
        {
            CompanyName = "test",
            RegistrationNumber = 12345,
            Email = "Test",
            Phone = "1234567"
        };

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.PostAsJsonAsync("/companies", company);
        var createCompanyResponse = await response.Content.ReadFromJsonAsync<CreateCompanyResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createCompanyResponse.Should().NotBeNull();
        createCompanyResponse.Success.Should().BeTrue();
    }

    [Fact]
    public async Task CreateCompany_Should_Give401_WhenNoToken()
    {
        var client = _companyFactory.CreateClient();
        var company = new CreateCompanyRequest
        {
            CompanyName = "test",
            RegistrationNumber = 12345,
            Email = "Test",
            Phone = "1234567"
        };

        var response = await client.PostAsJsonAsync("/companies", company);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateCompany_Should_Give401_WhenInvalidToken()
    {
        var client = _companyFactory.CreateClient();
        var token = "super-key";
        var company = new CreateCompanyRequest
        {
            CompanyName = "test",
            RegistrationNumber = 12345,
            Email = "Test",
            Phone = "1234567"
        };
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.PostAsJsonAsync("/companies", company);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateCompany_Should_Give400_WhenRequestModelInvalid()
    {
        var client = _companyFactory.CreateClient();
        var token = JwtGenerator.GenerateToken();

        var company = new CreateCompanyRequest
        {
            CompanyName = null,
            RegistrationNumber = 12345,
            Email = "Test",
            Phone = "1234567"
        };

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.PostAsJsonAsync("/companies", company);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateCompany_Should_Give400_WhenIdIsZero()
    {
        var client = _companyFactory.CreateClient();
        var token = JwtGenerator.GenerateToken();

        var id = 0;

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        var response = await client.GetAsync($"/companies/{id}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Task.CompletedTask;
}