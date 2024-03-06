using System.Net;

using FluentAssertions;

using WebApi;
using WebApi.Configuration;

namespace IntegrationTests;

public class OrdersControllerTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory _factory;

    public OrdersControllerTests(ApiWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetOrders_When_TenantIdHeader_IsMissing_Then_Return_BadRequest()
    {
        // arrange
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/orders");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData("x")]
    [InlineData("y")]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("2")]
    public async Task GetOrders_When_TenantIdHeader_IsNotValid_Then_Return_BadRequest(string tenantId)
    {
        // arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add(Constants.TenantIdHeaderName, tenantId);

        // act
        var response = await client.GetAsync("/orders");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Theory]
    [InlineData("fr")]
    [InlineData("Fr")]
    [InlineData("FR")]
    [InlineData("be")]
    [InlineData("Be")]
    [InlineData("BE")]
    public async Task GetOrders_When_TenantIdHeader_IsValid_Then_Return_Ok(string tenantId)
    {
        // arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add(Constants.TenantIdHeaderName, tenantId);

        // act
        var response = await client.GetAsync("/orders");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}