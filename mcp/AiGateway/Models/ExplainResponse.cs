namespace McpCodeExplainer.Models;

public sealed record ExplainResponse(
    string TechnicalExplanation,
    string BeginnerExplanation,
    string CombinedSummary);
