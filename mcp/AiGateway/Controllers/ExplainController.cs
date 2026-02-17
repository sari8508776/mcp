using Microsoft.AspNetCore.Mvc;
using McpCodeExplainer.Models;
using McpCodeExplainer.Services;

namespace McpCodeExplainer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExplainController : ControllerBase
{
    private readonly McpExplainOrchestrator _orchestrator;

    public ExplainController(McpExplainOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    [HttpPost("explain")]
    public async Task<ActionResult<ExplainResponse>> Explain([FromBody] ExplainRequest request, CancellationToken ct)
    {
        var result = await _orchestrator.ExplainAsync(request, ct);
        return Ok(result);
    }
}