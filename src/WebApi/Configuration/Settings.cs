namespace WebApi.Configuration;

public sealed record Settings
{
    public const string SectionName = "Settings";
    
    public bool AcceptNumericValues { get; init; }
    public string[] ExcludedPaths { get; init; } = ["swagger"];
}