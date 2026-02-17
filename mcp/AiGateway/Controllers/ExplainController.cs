using McpCodeExplainer.Models;
using McpCodeExplainer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace McpCodeExplainer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExplainController : ControllerBase
{
    private readonly McpExplainOrchestrator _orchestrator;

    public ExplainController(McpExplainOrchestrator orchestrator)
        => _orchestrator = orchestrator;

    //[EnableRateLimiting("mcp-ai-limit")]
    [HttpPost]
    public async Task<ActionResult<ExplainResponse>> Explain(
     [FromBody] ExplainRequest req,
     CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Subject  ))
            return BadRequest("Code is required.");
        if (string.IsNullOrWhiteSpace(req.Addressee))
            return BadRequest("Code is required.");

        var result = await _orchestrator.ExplainAsync(req, ct);
        return Ok(result);
    }

}
