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
        var company = new CreateCompanyRequest
        {
            CompanyName = "test",
            RegistrationNumber = 12345,
            Email = "Test",
            Phone = "1234567"
        };

        var response = await _companyFactory.HttpClient.PostAsJsonAsync("/companies", company);
        var createCompanyResponse = await response.Content.ReadFromJsonAsync<CreateCompanyResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createCompanyResponse.Should().NotBeNull();
        createCompanyResponse.Success.Should().BeTrue();
    }

    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync() => Task.CompletedTask;
}