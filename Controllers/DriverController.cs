using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class DriverController : ControllerBase
{
    private readonly IDriverService _driverService;

    public DriverController(IDriverService driverService)
    {
        _driverService = driverService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDrivers()
    {
        var drivers = await _driverService.GetDriversAsync();
        return Ok(drivers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDriver(int id)
    {
        try
        {
            var driver = await _driverService.GetDriverByIdAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return Ok(driver);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateDriver([FromBody] Driver driver)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdDriver = await _driverService.CreateDriverAsync(driver);
        return CreatedAtAction(nameof(GetDriver), new { id = createdDriver.DriverId }, createdDriver);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDriver(int id, [FromBody] Driver driver)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _driverService.UpdateDriverAsync(id, driver);
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
        //     return StatusCode(500, "An error occurred while updating the driver.");
        // }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDriver(int id)
    {
        try
        {
            await _driverService.DeleteDriverAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
