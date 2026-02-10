using McpCodeExplainer.Models;
using OpenAI;
using OpenAI.Chat;

namespace McpCodeExplainer.Services;

public sealed class McpExplainOrchestrator
{
    private readonly OpenAIClient _client;

    public McpExplainOrchestrator()
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("Missing OPENAI_API_KEY env var.");

        _client = new OpenAIClient(apiKey);
    }

    public async Task<ExplainResponse> ExplainAsync(ExplainRequest req, CancellationToken ct)
    {
        // Agent 1: טכני
        var technicalPrompt =
$"""
You are a senior software engineer.
Explain what the following {req.Language} code does, focusing on:
- control flow
- key concepts (async/await, LINQ, etc.)
- potential pitfalls
Return concise bullet points.

CODE:
{req.Code}
""";

        // Agent 2: למתחילות
        var beginnerPrompt =
$"""
You are a patient teacher for beginners.
Explain the same {req.Language} code in very simple language:
- no jargon if possible
- add a tiny analogy
- 5-8 short sentences max
Return in Hebrew.

CODE:
{req.Code}
""";

        // שתי קריאות (שני Agents)
        var technicalTask = CreateTextAsync(technicalPrompt, ct);
        var beginnerTask = CreateTextAsync(beginnerPrompt, ct);

        await Task.WhenAll(technicalTask, beginnerTask);

        var technical = technicalTask.Result.Trim();
        var beginner = beginnerTask.Result.Trim();

        // איחוד “קל” (ללא קריאה שלישית)
        var combined =
$"""
סיכום מאוחד:
- טכנית: {Shorten(technical, 350)}
- למתחילות: {Shorten(beginner, 350)}
""";

        return new ExplainResponse(technical, beginner, combined);
    }

    private async Task<string> CreateTextAsync(string prompt, CancellationToken ct)
    {
        var chatClient = _client.GetChatClient("gpt-4o");
        var response = await chatClient.CompleteChatAsync(
            [new UserChatMessage(prompt)],
            cancellationToken: ct);

        return response.Value.Content[0].Text;
    }

    private static string Shorten(string s, int max)
        => s.Length <= max ? s : s[..max] + "…";
}
