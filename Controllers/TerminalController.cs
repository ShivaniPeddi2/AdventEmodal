using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TerminalController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TerminalController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTerminals()
    {
        var terminals = await _context.Terminals.ToListAsync();
        return Ok(terminals);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTerminal(int id)
    {
        var terminal = await _context.Terminals.FindAsync(id);
        if (terminal == null)
        {
            return NotFound();
        }
        return Ok(terminal);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTerminal([FromBody] Terminal terminal)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Terminals.Add(terminal);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTerminal), new { id = terminal.TerminalId }, terminal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTerminal(int id, [FromBody] Terminal terminal)
    {
        if (id != terminal.TerminalId)
        {
            return BadRequest();
        }

        var existingTerminal = await _context.Terminals.FindAsync(id);
        if (existingTerminal == null)
        {
            return NotFound();
        }

        existingTerminal.Address = terminal.Address;
        existingTerminal.State = terminal.State;
        existingTerminal.Pincode = terminal.Pincode;
        existingTerminal.Country = terminal.Country;
        existingTerminal.GateNo = terminal.GateNo;
        existingTerminal.RegistrationDate = terminal.RegistrationDate;

        _context.Entry(existingTerminal).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTerminal(int id)
    {
        var terminal = await _context.Terminals.FindAsync(id);
        if (terminal == null)
        {
            return NotFound();
        }

        _context.Terminals.Remove(terminal);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
