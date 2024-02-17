using FluentAssertions;

using WebApi;

namespace UnitTests;

public class TenantIdExtensionsTests
{
    [Theory]
    [InlineData("fr")]
    [InlineData("Fr")]
    [InlineData("FR")]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("3")]
    public void IsValidTenantId_When_NumericValues_Are_Accepted_Then_Should_TenantId_Be_Valid(string tenantId)
    {
        // arrange
        // act
        var isValid = tenantId.IsValidTenantId(acceptNumericValues: true);

        // assert
        isValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("fr")]
    [InlineData("Fr")]
    [InlineData("FR")]
    public void IsValidTenantId_When_NumericValues_Are_Not_Accepted_Then_Should_TenantId_Be_Valid(string tenantId)
    {
        // arrange
        // act
        var isValid = tenantId.IsValidTenantId(acceptNumericValues: false);

        // assert
        isValid.Should().BeTrue();
    }
    
    [Theory]
    [InlineData("x")]
    [InlineData("xyz")]
    [InlineData("0")]
    [InlineData("00")]
    [InlineData("10")]
    public void IsValidTenantId_When_NumericValues_Are_Accepted_Then_Should_TenantId_Not_Be_Valid(string tenantId)
    {
        // arrange
        // act
        var isValid = tenantId.IsValidTenantId(acceptNumericValues: true);

        // assert
        isValid.Should().BeFalse();
    }
    
    [Theory]
    [InlineData("x")]
    [InlineData("xyz")]
    [InlineData("0")]
    [InlineData("00")]
    [InlineData("10")]
    public void IsValidTenantId_When_NumericValues_Are_Not_Accepted_Then_Should_TenantId_Not_Be_Valid(string tenantId)
    {
        // arrange
        // act
        var isValid = tenantId.IsValidTenantId(acceptNumericValues: false);

        // assert
        isValid.Should().BeFalse();
    }
}