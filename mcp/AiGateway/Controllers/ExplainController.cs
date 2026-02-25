//using Microsoft.AspNetCore.Mvc;
//using McpCodeExplainer.Models;
//using McpCodeExplainer.Services;

//namespace McpCodeExplainer.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class ExplainController : ControllerBase
//{
//    private readonly McpExplainOrchestrator _orchestrator;

//    public ExplainController(McpExplainOrchestrator orchestrator)
//    {
//        _orchestrator = orchestrator;
//    }

//    [HttpPost("explain")]
//    public async Task<ActionResult<ExplainResponse>> Explain([FromBody] ExplainRequest request, CancellationToken ct)
//    {
//        var result = await _orchestrator.ExplainAsync(request, ct);
//        return Ok(result);
//    }
//}
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

    [HttpPost("generate-greeting")]
    public async Task<IActionResult> Explain([FromBody] ExplainRequest request, CancellationToken ct)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Subject))
        {
            return BadRequest("נא למלא את כל שדות החובה בבקשה.");
        }

        var result = await _orchestrator.ExplainAsync(request, ct);

        return Ok(result);
    }
}