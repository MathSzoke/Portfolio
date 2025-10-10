using Microsoft.AspNetCore.Mvc;
using Portfolio.AIAgent.Models;
using Portfolio.Application.Abstractions.AI;

namespace Portfolio.AIAgent.Controllers;

[ApiController]
[Route("api/v1/agent")]
public class AgentController(IAgentResponder agent) : ControllerBase
{
    [HttpPost("reply")]
    public async Task<IActionResult> GenerateReply([FromBody] AgentRequest req, CancellationToken ct)
    {
        var history = (req.History ?? []).Select(t => new ChatTurn(t.Role, t.Content));
        var reply = await agent.GenerateReplyAsync(history, req.Message, ct);
        return this.Ok(new AgentResponse { Reply = reply });
    }
}
