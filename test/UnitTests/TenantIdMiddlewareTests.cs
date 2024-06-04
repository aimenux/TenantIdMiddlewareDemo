using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using WebApi;
using WebApi.Configuration;

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
        using var body = new MemoryStream();
        var next = BuildRequestDelegate();
        var options = Options.Create(new Settings());
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = body,
            },
            Request =
            {
                Headers =
                {
                    [Constants.TenantIdHeaderName] = tenantId
                }
            }
        };
        
        var middleware = new TenantIdMiddleware(next, options);

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
        using var body = new MemoryStream();
        var next = BuildRequestDelegate();
        var options = Options.Create(new Settings());
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = body,
            },
            Request =
            {
                Headers =
                {
                    [Constants.TenantIdHeaderName] = tenantId
                }
            }
        };
        
        var middleware = new TenantIdMiddleware(next, options);

        // act
        await middleware.InvokeAsync(context);

        // assert
        context.Response.StatusCode.Should().Be(BadRequestStatusCode);
    }
    
    [Theory]
    [InlineData("/foo")]
    [InlineData("/bar")]
    [InlineData("/foobar")]
    [InlineData("/foo/bar")]
    public async Task InvokeAsync_When_Path_Is_Excluded_Then_Should_Return_Ok_StatusCode(string path)
    {
        // arrange
        using var body = new MemoryStream();
        var next = BuildRequestDelegate();
        var options = Options.Create(new Settings
        {
            ExcludedPaths = ["foo", "bar", "foobar"]
        });
        var context = new DefaultHttpContext
        {
            Response =
            {
                Body = body,
            },
            Request =
            {
                Path = new PathString(path)
            }
        };
        
        var middleware = new TenantIdMiddleware(next, options);

        // act
        await middleware.InvokeAsync(context);

        // assert
        context.Response.StatusCode.Should().Be(OkStatusCode);
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