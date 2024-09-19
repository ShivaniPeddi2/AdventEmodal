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
    public async Task<IActionResult> GetTerminals()
    {
        var terminals = await _terminalService.GetTerminalsAsync();
        return Ok(terminals);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTerminal(int id)
    {
        try
        {
            var terminal = await _terminalService.GetTerminalByIdAsync(id);
            if (terminal == null)
            {
                return NotFound();
            }
            return Ok(terminal);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTerminal([FromBody] Terminal terminal)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdTerminal = await _terminalService.CreateTerminalAsync(terminal);
        return CreatedAtAction(nameof(GetTerminal), new { id = createdTerminal.TerminalId }, createdTerminal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTerminal(int id, [FromBody] Terminal terminal)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _terminalService.UpdateTerminalAsync(id, terminal);
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

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTerminal(int id)
    {
        try
        {
            await _terminalService.DeleteTerminalAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
