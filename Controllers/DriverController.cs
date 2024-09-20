using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDriversAsync()
    {
        var drivers = await _driverService.GetDriversAsync();
        return Ok(drivers);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDriverByIdAsync(int id)
    {
        var driver = await _driverService.GetDriverByIdAsync(id);
        if (driver == null)
        {
            return NotFound();
        }
        return Ok(driver);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateDriverAsync([FromBody] Driver driver)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdDriver = await _driverService.CreateDriverAsync(driver);
        return CreatedAtAction(nameof(GetDriverByIdAsync), new { id = createdDriver.DriverId }, createdDriver);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDriverAsync(int id, [FromBody] Driver driver)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _driverService.UpdateDriverAsync(id, driver);
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
        //     return StatusCode(500, "An error occurred while updating the driver.");
        // }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDriverAsync(int id)
    {
        try
        {
            await _driverService.DeleteDriverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
