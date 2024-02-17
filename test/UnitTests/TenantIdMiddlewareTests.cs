using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

using WebApi;

namespace UnitTests;

public class TenantIdMiddlewareTests
{
    private const int OkStatusCode = 200;
    private const int BadRequestStatusCode = 400;
    
    [Theory]
    [InlineData("fr")]
    [InlineData("Fr")]
    [InlineData("FR")]
    [InlineData("be")]
    [InlineData("Be")]
    [InlineData("BE")]
    public async Task InvokeAsync_When_TenantId_Is_Valid_Then_Should_Return_Ok_StatusCode(string tenantId)
    {
        // arrange
        var logger = NullLogger<TenantIdMiddleware>.Instance;
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream(),
            },
            Request =
            {
                Headers =
                {
                    [Constants.TenantIdHeaderName] = tenantId
                }
            }
        };
        
        var next = BuildRequestDelegate();
        
        var middleware = new TenantIdMiddleware(next, logger);

        // act
        await middleware.InvokeAsync(context);

        // assert
        context.Response.StatusCode.Should().Be(OkStatusCode);
    }
    
    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("10")]
    [InlineData("99")]
    [InlineData("xyz")]
    public async Task InvokeAsync_When_TenantId_Is_Not_Valid_Then_Should_Return_BadRequest_StatusCode(string tenantId)
    {
        // arrange
        var logger = NullLogger<TenantIdMiddleware>.Instance;
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = new MemoryStream(),
            },
            Request =
            {
                Headers =
                {
                    [Constants.TenantIdHeaderName] = tenantId
                }
            }
        };
        
        var next = BuildRequestDelegate();
        
        var middleware = new TenantIdMiddleware(next, logger);

        // act
        await middleware.InvokeAsync(context);

        // assert
        context.Response.StatusCode.Should().Be(BadRequestStatusCode);
    }

    private static RequestDelegate BuildRequestDelegate()
    {
        return ctx =>
        {
            ctx.Response.StatusCode = 200;
            return Task.CompletedTask;
        };
    }
}