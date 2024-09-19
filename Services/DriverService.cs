using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DriverService : IDriverService
{
    private readonly ApplicationDbContext _context;

    public DriverService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Driver>> GetDriversAsync()
    {
        return await _context.Drivers
            .Include(d => d.TruckCompany)
            .ToListAsync();
    }

    public async Task<Driver> GetDriverByIdAsync(int id)
    {
        return await _context.Drivers
            .Include(d => d.TruckCompany)
            .FirstOrDefaultAsync(d => d.DriverId == id);
    }

    public async Task<Driver> CreateDriverAsync(Driver driver)
    {
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();
        return driver;
    }

    public async Task UpdateDriverAsync(int id, Driver driver)
    {
        if (id != driver.DriverId)
        {
            throw new ArgumentException("Driver ID mismatch.");
        }

        var existingDriver = await _context.Drivers.FindAsync(id);
        if (existingDriver == null)
        {
            throw new KeyNotFoundException("Driver not found.");
        }

        existingDriver.Name = driver.Name;
        existingDriver.LicenseNumber = driver.LicenseNumber;
        existingDriver.PhoneNumber = driver.PhoneNumber;
        existingDriver.TruckCompanyId = driver.TruckCompanyId;

        _context.Entry(existingDriver).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDriverAsync(int id)
    {
        var driver = await _context.Drivers.FindAsync(id);
        if (driver == null)
        {
            throw new KeyNotFoundException("Driver not found.");
        }

        _context.Drivers.Remove(driver);
        await _context.SaveChangesAsync();
    }
}
