using System.Collections.Generic;
using System.Threading.Tasks;

public interface IDriverService
{
    Task<IEnumerable<Driver>> GetDriversAsync();
    Task<Driver?> GetDriverByIdAsync(int id);
    Task<Driver> CreateDriverAsync(Driver driver);
    Task UpdateDriverAsync(int id, Driver driver);
    Task DeleteDriverAsync(int id);
}
