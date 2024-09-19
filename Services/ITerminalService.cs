using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITerminalService
{
    Task<IEnumerable<Terminal>> GetTerminalsAsync();
    Task<Terminal> GetTerminalByIdAsync(int id);
    Task<Terminal> CreateTerminalAsync(Terminal terminal);
    Task UpdateTerminalAsync(int id, Terminal terminal);
    Task DeleteTerminalAsync(int id);
}
