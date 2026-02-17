////////using McpCodeExplainer.Services;
////////using Microsoft.AspNetCore.RateLimiting;
////////using System.Threading.RateLimiting;
////////var builder = WebApplication.CreateBuilder(args);

////////builder.Services.AddControllers();
////////builder.Services.AddEndpointsApiExplorer();
////////builder.Services.AddSwaggerGen();

////////var app = builder.Build();

////////// Configure the HTTP request pipeline.
//////////if (app.Environment.IsDevelopment())
//////////{
//////////    app.UseSwagger();
//////////    app.UseSwaggerUI();
//////////}

//////////app.UseHttpsRedirection();



////////builder.Services.AddSingleton<McpExplainOrchestrator>();

////////app.UseSwagger();
////////app.UseSwaggerUI();

////////app.MapControllers();

////////app.Run();
//////using McpCodeExplainer.Services;

//////var builder = WebApplication.CreateBuilder(args);

//////// --- כאן רושמים את השירותים (לפני ה-Build!) ---

//////// 1. רישום HttpClient - חובה כי ה-Orchestrator דורש אותו ב-Constructor
//////builder.Services.AddHttpClient();

//////// 2. רישום ה-Orchestrator שלך
//////builder.Services.AddSingleton<McpExplainOrchestrator>();

//////// ----------------------------------------------

//////var app = builder.Build(); // <--- אחרי השורה הזו, הכל הופך ל-Read-Only

//////// הגדרות ה-Middleware (Routing, וכו')
//////app.MapControllers();

//////app.Run();
////using McpCodeExplainer.Services;

////var builder = WebApplication.CreateBuilder(args);

////// --- 1. רישום שירותי המערכת ---

////// השורה שחסרה לך ומייצרת את השגיאה:
////builder.Services.AddControllers();

////// רישום ה-HttpClient (נחוץ ל-Gemini)
////builder.Services.AddHttpClient();

////// רישום ה-Orchestrator שלך
////builder.Services.AddSingleton<McpExplainOrchestrator>();

////// --- 2. בניית האפליקציה ---
////var app = builder.Build();

////// --- 3. הגדרת ניתובים (Routing) ---
////// עכשיו השורה הזו תעבוד בלי לקרוס:
////app.MapControllers();

////app.Run();
//using McpCodeExplainer.Services;

//var builder = WebApplication.CreateBuilder(args);

//// --- 1. רישום שירותים (לפני ה-Build) ---
//builder.Services.AddControllers();
//builder.Services.AddHttpClient();
//builder.Services.AddSingleton<McpExplainOrchestrator>();

//// ** שורות חדשות עבור Swagger **
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// ------------------------------------------

//var app = builder.Build();

//// --- 2. הגדרת Pipeline (אחרי ה-Build) ---

//// הפעלת Swagger רק בסביבת פיתוח (סטנדרט של דוט נט)
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(); // זה מה שמייצר את הדף הוויזואלי
//}

//app.MapControllers();

//app.Run();
//DotNetEnv.Env.Load();
// 1. קודם כל טוענים את הקובץ (ממש בשורה הראשונה!)
using McpCodeExplainer.Services;
using DotNetEnv;
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// 2. רישום שירותים
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<McpExplainOrchestrator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3. הגדרות Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();