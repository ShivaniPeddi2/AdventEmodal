using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class DriverController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DriverController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetDrivers()
    {
        var drivers = await _context.Drivers
            .Include(d => d.TruckCompany)
            .ToListAsync();
        return Ok(drivers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDriver(int id)
    {
        var driver = await _context.Drivers
            .Include(d => d.TruckCompany)
            .FirstOrDefaultAsync(d => d.DriverId == id);

        if (driver == null)
        {
            return NotFound();
        }
        return Ok(driver);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDriver([FromBody] Driver driver)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDriver), new { id = driver.DriverId }, driver);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDriver(int id, [FromBody] Driver driver)
    {
        if (id != driver.DriverId)
        {
            return BadRequest();
        }

        var existingDriver = await _context.Drivers.FindAsync(id);
        if (existingDriver == null)
        {
            return NotFound();
        }

        existingDriver.Name = driver.Name;
        existingDriver.LicenseNumber = driver.LicenseNumber;
        existingDriver.PhoneNumber = driver.PhoneNumber;
        existingDriver.TruckCompanyId = driver.TruckCompanyId;

        _context.Entry(existingDriver).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
public async Task<IActionResult> DeleteDriver(int id)
{
    // Log the ID to verify it's correct
    Console.WriteLine($"Attempting to delete driver with ID: {id}");

    var driver = await _context.Drivers.FindAsync(id);
    if (driver == null)
    {
        // Log not found error
        Console.WriteLine($"Driver with ID {id} not found.");
        return NotFound();
    }

    _context.Drivers.Remove(driver);
    await _context.SaveChangesAsync();

    return NoContent();
}

}
