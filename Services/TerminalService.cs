using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TerminalService : ITerminalService
{
    private readonly ApplicationDbContext _context;

    public TerminalService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Terminal>> GetTerminalsAsync()
    {
        return await _context.Terminals.ToListAsync();
    }

    public async Task<Terminal> GetTerminalByIdAsync(int id)
    {
        return await _context.Terminals.FindAsync(id);
    }

    public async Task<Terminal> CreateTerminalAsync(Terminal terminal)
    {
        _context.Terminals.Add(terminal);
        await _context.SaveChangesAsync();
        return terminal;
    }

    public async Task UpdateTerminalAsync(int id, Terminal terminal)
    {
        if (id != terminal.TerminalId)
        {
            throw new ArgumentException("Terminal ID mismatch.");
        }

        var existingTerminal = await _context.Terminals.FindAsync(id);
        if (existingTerminal == null)
        {
            throw new KeyNotFoundException("Terminal not found.");
        }

        existingTerminal.Address = terminal.Address;
        existingTerminal.State = terminal.State;
        existingTerminal.Pincode = terminal.Pincode;
        existingTerminal.Country = terminal.Country;
        existingTerminal.GateNo = terminal.GateNo;
        existingTerminal.RegistrationDate = terminal.RegistrationDate;

        _context.Entry(existingTerminal).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTerminalAsync(int id)
    {
        var terminal = await _context.Terminals.FindAsync(id);
        if (terminal == null)
        {
            throw new KeyNotFoundException("Terminal not found.");
        }

        _context.Terminals.Remove(terminal);
        await _context.SaveChangesAsync();
    }
}
