//////////using McpCodeExplainer.Models;
//////////using GEMINI_API_KEY;
//////////using Gemini.Chat;

//////////namespace McpCodeExplainer.Services;

//////////public sealed class McpExplainOrchestrator
//////////{
//////////    private readonly GEMINI_API_KEY;

//////////    public McpExplainOrchestrator()
//////////    {
//////////        var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
//////////        if (string.IsNullOrWhiteSpace(apiKey))
//////////            throw new InvalidOperationException("Missing GEMINI_API_KEY env var.");

//////////        _client = new OpenAIClient(apiKey);
//////////    }

//////////    public async Task<ExplainResponse> ExplainAsync(ExplainRequest req, CancellationToken ct)
//////////    {
//////////        // Agent 1: טכני
//////////        var technicalPrompt =
//////////$"""
//////////Write me a personal, heartfelt and moving greeting
//////////of no more than 50 words, in Jewish style, with rhymes,
//////////in this language: {req.Language}, 
//////////on the subject: {req.subject},
//////////in a high language,
//////////""";

//////////        // Agent 2: למתחילות
//////////        var beginnerPrompt =
//////////$"""
//////////You are a patient teacher for beginners.
//////////Explain the same {req.Language} code in very simple language:
//////////- no jargon if possible
//////////- add a tiny analogy
//////////- 5-8 short sentences max
//////////Return in Hebrew.

//////////CODE:
//////////{req.Code}
//////////""";

//////////        // שתי קריאות (שני Agents)
//////////        var technicalTask = CreateTextAsync(technicalPrompt, ct);
//////////        var beginnerTask = CreateTextAsync(beginnerPrompt, ct);

//////////        await Task.WhenAll(technicalTask, beginnerTask);

//////////        var technical = technicalTask.Result.Trim();
//////////        var beginner = beginnerTask.Result.Trim();

//////////        // איחוד “קל” (ללא קריאה שלישית)
//////////        var combined =
//////////$"""
//////////סיכום מאוחד:
//////////- טכנית: {Shorten(technical, 350)}
//////////- למתחילות: {Shorten(beginner, 350)}
//////////""";

//////////        return new ExplainResponse(technical, beginner, combined);
//////////    }

//////////    private async Task<string> CreateTextAsync(string prompt, CancellationToken ct)
//////////    {
//////////        var chatClient = _client.GetChatClient("gpt-4o");
//////////        var response = await chatClient.CompleteChatAsync(
//////////            [new UserChatMessage(prompt)],
//////////            cancellationToken: ct);

//////////        return response.Value.Content[0].Text;
//////////    }

//////////    private static string Shorten(string s, int max)
//////////        => s.Length <= max ? s : s[..max] + "…";
//////////}
////////using Google.GenerativeAI; // וודא שהתקנת את החבילה ב-NuGet
////////using McpCodeExplainer.Models;
////////using System;
////////using System.Threading;
////////using System.Threading.Tasks;

////////namespace McpCodeExplainer.Services;

////////public sealed class McpExplainOrchestrator
////////{
////////    private readonly GenerativeModel _model;

////////    public McpExplainOrchestrator()
////////    {
////////        // שליפת מפתח ה-API מהסביבה
////////        var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
////////        if (string.IsNullOrWhiteSpace(apiKey))
////////            throw new InvalidOperationException("Missing GEMINI_API_KEY env var.");

////////        // אתחול ה-Client של Gemini
////////        // הערה: ב-SDK הרשמי של גוגל, נשתמש ב-GeminiClient או ישירות ב-GenerativeModel
////////        var client = new GeminiClient(apiKey);

////////        // בחירת המודל - Gemini 1.5 Pro מומלץ ליצירתיות וחריזה (הברכה היהודית)
////////        _model = client.GenerativeModel("gemini-1.5-pro");
////////    }

////////    public async Task<ExplainResponse> ExplainAsync(ExplainRequest req, CancellationToken ct)
////////    {
////////        // Agent 1: ברכה יהודית מרגשת (הלוגיקה המקורית שלך)
////////        var technicalPrompt =
////////$"""
////////Write me a personal, heartfelt and moving greeting
////////of no more than 50 words, in Jewish style, with rhymes,
////////in this language: {req.Language}, 
////////on the subject: {req.Subject},
////////in a high language.
////////""";

////////        // Agent 2: הסבר למתחילות (פשוט וברור)
////////        var beginnerPrompt =
////////$"""
////////You are a patient teacher for beginners.
////////Explain the same {req.Language} code in very simple language:
////////- no jargon if possible
////////- add a tiny analogy
////////- 5-8 short sentences max
////////Return in Hebrew.

////////CODE:
////////{req.Code}
////////""";

////////        // הרצה במקביל של שני הסוכנים (Agents) לביצועים אופטימליים
////////        var technicalTask = _model.GenerateContentAsync(technicalPrompt, ct);
////////        var beginnerTask = _model.GenerateContentAsync(beginnerPrompt, ct);

////////        // המתנה לסיום שני המשימות
////////        await Task.WhenAll(technicalTask, beginnerTask);

////////        // חילוץ התוצאות מהתגובה של Gemini
////////        var technical = technicalTask.Result.Text?.Trim() ?? "לא התקבלה תשובה";
////////        var beginner = beginnerTask.Result.Text?.Trim() ?? "לא התקבלה תשובה";

////////        // איחוד התשובות לסיכום קצר
////////        var combined =
////////$"""
////////סיכום מאוחד:
////////- הברכה: {Shorten(technical, 350)}
////////- למתחילות: {Shorten(beginner, 350)}
////////""";

////////        return new ExplainResponse(technical, beginner, combined);
////////    }

////////    /// <summary>
////////    /// מתודה עזר לקיצור טקסט ארוך מדי
////////    /// </summary>
////////    private static string Shorten(string s, int max)
////////        => s.Length <= max ? s : s[..max] + "…";
////////}
//////using Google.GenerativeAI;
//////using McpCodeExplainer.Models;

//////namespace McpCodeExplainer.Services;

//////public sealed class McpExplainOrchestrator
//////{
//////    private readonly GenerativeModel _model;

//////    public McpExplainOrchestrator()
//////    {
//////        // 1. קריאת המפתח ששמרת במערכת
//////        var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
//////        if (string.IsNullOrWhiteSpace(apiKey))
//////            throw new InvalidOperationException("Missing GEMINI_API_KEY. וודא שהגדרת את המפתח במשתני הסביבה.");

//////        // 2. אתחול הלקוח
//////        var client = new GeminiClient(apiKey);

//////        // 3. שימוש במודל Flash - הוא הכי מהיר וחינמי לגמרי במכסות גבוהות
//////        _model = client.GenerativeModel("gemini-1.5-flash");
//////    }

//////    public async Task<ExplainResponse> ExplainAsync(ExplainRequest req, CancellationToken ct)
//////    {
//////        var greetingPrompt = $"""
//////Write me a personal, heartfelt and moving greeting
//////of no more than 50 words, in Jewish style, with rhymes,
//////in this language: {req.Language},
//////on the subject: {req.Subject},

//////The greeting is addressed to: {req.addressee}.

//////in a high language.
//////""";



//////        try
//////        {
//////            var task1 = _model.GenerateContentAsync(greetingPrompt, ct);

//////            await Task.WhenAll(task1);

//////            var greeting = task1.Result.Text ?? "לא הצלחתי לחרוז הפעם...";



//////            return new ExplainResponse(greeting );
//////        }
//////        catch (Exception ex)
//////        {
//////            return new ExplainResponse("שגיאה: {ex.Message}");
//////        }
//////    }

//////    private static string Shorten(string s, int max) => s.Length <= max ? s : s[..max] + "...";
//////}

////using System.Text;
////using System.Text.Json;
////using McpCodeExplainer.Models;

////namespace McpCodeExplainer.Services;

////public sealed class McpExplainOrchestrator
////{
////    private readonly HttpClient _httpClient;
////    private readonly string GEMINI_API_KEY;
////    private const string GeminiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

////    public McpExplainOrchestrator(HttpClient httpClient)
////    {
////        _httpClient = httpClient;

////        GEMINI_API_KEY = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

////        if (string.IsNullOrWhiteSpace(GEMINI_API_KEY))
////            throw new InvalidOperationException("Missing GEMINI_API_KEY env var.");
////    }

////    public async Task<ExplainResponse> ExplainAsync(ExplainRequest req, CancellationToken ct)
////    {
////        var greetingPrompt = $"""
////        Write me a personal, heartfelt and moving greeting
////        of no more than 50 words, in Jewish style, with rhymes,
////        in this language: {req.Language},
////        on the subject: {req.Subject},
////        The greeting is addressed to: {req.Addressee}.
////        in a high language.
////        """;

////        try
////        {
////            // יצירת גוף הבקשה בפורמט שגוגל דורשת
////            var requestBody = new
////            {
////                contents = new[]
////                {
////                    new { parts = new[] { new { text = greetingPrompt } } }
////                }
////            };

////            var jsonPayload = JsonSerializer.Serialize(requestBody);
////            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

////            // שליחת הבקשה - המפתח עובר ב-Query String
////            var response = await _httpClient.PostAsync($"{GeminiUrl}?key={_apiKey}", content, ct);
////            response.EnsureSuccessStatusCode();

////            var responseJson = await response.Content.ReadAsStringAsync(ct);

////            // חילוץ הטקסט מתוך ה-JSON המורכב של גוגל
////            var greeting = ExtractTextFromJson(responseJson);

////            return new ExplainResponse(greeting);
////        }
////        catch (Exception ex)
////        {
////            return new ExplainResponse($"שגיאה: {ex.Message}");
////        }
////    }

////    private string ExtractTextFromJson(string json)
////    {
////        using var doc = JsonDocument.Parse(json);
////        try
////        {
////            // המבנה של גוגל: candidates[0].content.parts[0].text
////            return doc.RootElement
////                .GetProperty("candidates")[0]
////                .GetProperty("content")
////                .GetProperty("parts")[0]
////                .GetProperty("text")
////                .GetString() ?? "לא התקבל טקסט";
////        }
////        catch
////        {
////            return "שגיאה בפענוח התשובה מהשרת";
////        }
////    }
////}

//using System.Text;
//using System.Text.Json;
//using McpCodeExplainer.Models;

//namespace McpCodeExplainer.Services;

//public sealed class McpExplainOrchestrator
//{
//    private readonly HttpClient _httpClient;
//    private readonly string _apiKey;
//    // כתובת ה-API של Gemini 1.5 Flash
//    //private const string GeminiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";
//    private const string GeminiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent";    // ה-HttpClient מוזרק כאן דרך ה-Dependency Injection
//    public McpExplainOrchestrator(HttpClient httpClient)
//    {
//        _httpClient = httpClient;

//        // שליפת המפתח ממשתני הסביבה (נטען מה-env ב-Program.cs)
//        _apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

//        if (string.IsNullOrWhiteSpace(_apiKey))
//            throw new InvalidOperationException("Missing GEMINI_API_KEY env var.");
//    }

//    public async Task<ExplainResponse> ExplainAsync(ExplainRequest req, CancellationToken ct)
//    {
//        // בניית הפרומפט להוראה ל-AI
//        var greetingPrompt = $"""
//        Write me a personal, heartfelt and moving greeting
//        of no more than 50 words, in Jewish style, with rhymes,
//        in this language: {req.Language},
//        on the subject: {req.Subject}.
//        The greeting is addressed to: {req.Addressee}.
//        in a high language.
//        """;

//        try
//        {
//            // 

//            // יצירת גוף הבקשה בפורמט JSON שגוגל דורשת
//            var requestBody = new
//            {
//                contents = new[]
//                {
//                    new { parts = new[] { new { text = greetingPrompt } } }
//                }
//            };

//            var jsonPayload = JsonSerializer.Serialize(requestBody);
//            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

//            // שליחת הבקשה - המפתח עובר ב-Query String של ה-URL
//            var response = await _httpClient.PostAsync($"{GeminiUrl}?key={_apiKey}", content, ct);

//            // בדיקה שהבקשה הצליחה
//            response.EnsureSuccessStatusCode();

//            var responseJson = await response.Content.ReadAsStringAsync(ct);

//            // 

//            // חילוץ הטקסט מתוך ה-JSON המורכב שגוגל מחזירה
//            var greeting = ExtractTextFromJson(responseJson);

//            // החזרת התשובה בפורמט ה-Record המצופה
//            return new ExplainResponse(greeting);
//        }
//        catch (Exception ex)
//        {
//            // החזרת הודעת שגיאה במקרה של תקלה
//            return new ExplainResponse($"שגיאה: {ex.Message}");
//        }
//    }

//    // פונקציית עזר לחילוץ הטקסט מתוך ה-JSON של התשובה
//    private string ExtractTextFromJson(string json)
//    {
//        using var doc = JsonDocument.Parse(json);
//        try
//        {
//            // המבנה של גוגל: candidates[0].content.parts[0].text
//            return doc.RootElement
//                .GetProperty("candidates")[0]
//                .GetProperty("content")
//                .GetProperty("parts")[0]
//                .GetProperty("text")
//                .GetString() ?? "לא התקבל טקסט";
//        }
//        catch
//        {
//            return "שגיאה בפענוח התשובה מהשרת";
//        }
//    }
//}
using System.Text;
using System.Text.Json;
using McpCodeExplainer.Models;

namespace McpCodeExplainer.Services;

public sealed class McpExplainOrchestrator
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    // כתובת ה-API של Gemini 1.5 Flash - וודא שאין רווחים בסוף המחרוזת
    // האופציה המומלצת ביותר כרגע ב-v1beta
    private const string GeminiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent";
    public McpExplainOrchestrator(HttpClient httpClient)
    {
        _httpClient = httpClient;

        // שליפת המפתח ממשתני הסביבה (נטען מה-env ב-Program.cs)
        var rawKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

        if (string.IsNullOrWhiteSpace(rawKey))
            throw new InvalidOperationException("Missing GEMINI_API_KEY env var. וודא שקובץ ה-.env מוגדר כ-'Copy if newer'.");

        // ניקוי רווחים מיותרים מהמפתח למניעת שגיאות URL
        _apiKey = rawKey.Trim();
    }

    public async Task<ExplainResponse> ExplainAsync(ExplainRequest req, CancellationToken ct)
    {
        // בניית הפרומפט
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
            // יצירת גוף הבקשה בפורמט שגוגל דורשת
            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = greetingPrompt } } }
                }
            };

            var jsonPayload = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // בניית ה-URL הסופי עם המפתח
            var finalUrl = $"{GeminiUrl}?key={_apiKey}";

            // שליחת הבקשה
            var response = await _httpClient.PostAsync(finalUrl, content, ct);

            // אם הסטטוס אינו הצלחה (למשל 404, 400, 403)
            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync(ct);
                // זה יחזיר לך ל-Swagger את הסיבה המדויקת שגוגל דחתה את הבקשה
                return new ExplainResponse($"שגיאת שרת גוגל ({response.StatusCode}): {errorDetails}");
            }

            var responseJson = await response.Content.ReadAsStringAsync(ct);

            // חילוץ הטקסט מתוך ה-JSON
            var greeting = ExtractTextFromJson(responseJson);

            return new ExplainResponse(greeting);
        }
        catch (Exception ex)
        {
            // טיפול בשגיאות תקשורת או קריסה פנימית
            return new ExplainResponse($"שגיאה כללית בקוד: {ex.Message}");
        }
    }

    private string ExtractTextFromJson(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            // המבנה של גוגל: candidates[0].content.parts[0].text
            return doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "לא התקבל טקסט מהמודל";
        }
        catch (Exception)
        {
            return "שגיאה בפענוח ה-JSON שהתקבל מגוגל.";
        }
    }
}