using System.Text;
using System.Text.Json;
using McpCodeExplainer.Models;

namespace McpCodeExplainer.Services;

public sealed class McpExplainOrchestrator
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    private const string GeminiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

    public McpExplainOrchestrator(HttpClient httpClient)
    {
        _httpClient = httpClient;
        var rawKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
        _apiKey = rawKey?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(_apiKey))
            throw new InvalidOperationException("GEMINI_API_KEY is missing in environment variables.");
    }

    public async Task<ExplainResponse> ExplainAsync(ExplainRequest req, CancellationToken ct)
    {
        var greetingPrompt = $"""
        Write me a personal, heartfelt and moving greeting
        of no more than 50 words, in Jewish style, with rhymes,
        in this language: {req.Language},
        on the subject: {req.Subject}.
        The greeting is addressed to: {req.Addressee}.
        in a high language.
        """;

        try
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = greetingPrompt } } }
                }
            };

            var jsonPayload = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var finalUrl = $"{GeminiUrl}?key={_apiKey}";
            var response = await _httpClient.PostAsync(finalUrl, content, ct);
            var responseJson = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                return new ExplainResponse($"שגיאת שרת גוגל ({response.StatusCode}): {responseJson}");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                return new ExplainResponse("המכסה החינמית של גוגל הסתיימה לדקה זו. אנא נסי שוב בעוד כחצי דקה.");
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync(ct);
                return new ExplainResponse($"שגיאת שרת גוגל ({response.StatusCode})");
            }
            var greeting = ExtractTextFromJson(responseJson);
            return new ExplainResponse(greeting);
        }
        catch (Exception ex)
        {
            return new ExplainResponse($"שגיאה פנימית: {ex.Message}");
        }
    }

    private string ExtractTextFromJson(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "לא התקבל תוכן";
        }
        catch
        {
            return "שגיאה בפענוח התשובה מגוגל";
        }
    }
}