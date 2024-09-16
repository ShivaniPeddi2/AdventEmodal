using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetDrivers()
    {
        // Only Admins can retrieve all drivers, but users might be allowed to see only their associated drivers if needed
        if (User.IsInRole("Admin"))
        {
            var drivers = await _context.Drivers
                .Include(d => d.TruckCompany)
                .ToListAsync();
            return Ok(drivers);
        }
        else
        {
            // Regular users can only see drivers associated with their own TruckCompany
            var userTruckCompanyId = GetUserTruckCompanyId();
            var drivers = await _context.Drivers
                .Where(d => d.TruckCompanyId == userTruckCompanyId)
                .Include(d => d.TruckCompany)
                .ToListAsync();
            return Ok(drivers);
        }
    }

    [Authorize]
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

        // Admins can access any driver; users can only access drivers from their own TruckCompany
        if (User.IsInRole("Admin") || driver.TruckCompanyId == GetUserTruckCompanyId())
        {
            return Ok(driver);
        }

        return Forbid();
    }

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDriver(int id)
    {
        var driver = await _context.Drivers.FindAsync(id);
        if (driver == null)
        {
            return NotFound();
        }

        _context.Drivers.Remove(driver);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private int GetUserTruckCompanyId()
    {
        // Assuming you store the TruckCompanyId in the claims, adjust as needed
        var truckCompanyIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TruckCompanyId");
        return int.Parse(truckCompanyIdClaim?.Value ?? "0");
    }
}
