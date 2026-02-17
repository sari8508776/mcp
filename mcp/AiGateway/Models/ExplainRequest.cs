namespace McpCodeExplainer.Models;

public sealed record ExplainRequest(string Subject,string Addressee, string Language = "Hebrew");
