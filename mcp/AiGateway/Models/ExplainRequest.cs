
namespace McpCodeExplainer.Models;

public sealed record ExplainRequest(
    string Subject,
    string Language,
    string Addressee,
    string? Code
);