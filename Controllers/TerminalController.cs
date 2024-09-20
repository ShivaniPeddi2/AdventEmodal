using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TerminalController : ControllerBase
{
    private readonly ITerminalService _terminalService;

    public TerminalController(ITerminalService terminalService)
    {
        _terminalService = terminalService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetTerminalsAsync()
    {
        var terminals = await _terminalService.GetTerminalsAsync();
        return Ok(terminals);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTerminalByIdAsync(int id)
    {
        var terminal = await _terminalService.GetTerminalByIdAsync(id);
        if (terminal == null)
        {
            return NotFound();
        }
        return Ok(terminal);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateTerminalAsync([FromBody] Terminal terminal)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdTerminal = await _terminalService.CreateTerminalAsync(terminal);
        return CreatedAtAction(nameof(GetTerminalByIdAsync), new { id = createdTerminal.TerminalId }, createdTerminal);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateTerminalAsync(int id, [FromBody] Terminal terminal)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _terminalService.UpdateTerminalAsync(id, terminal);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        // catch (DbUpdateException)
        // {
        //     return StatusCode(500, "An error occurred while updating the terminal.");
        // }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTerminalAsync(int id)
    {
        try
        {
            await _terminalService.DeleteTerminalAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
