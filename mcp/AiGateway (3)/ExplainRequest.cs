namespace McpCodeExplainer.Models;

public sealed record ExplainRequest(string Code, string Language = "csharp");
