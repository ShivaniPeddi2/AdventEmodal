using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ContainerController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ContainerController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetContainers()
    {
        var containers = await _context.Containers.ToListAsync();
        return Ok(containers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContainer(int id)
    {
        var container = await _context.Containers.FindAsync(id);
        if (container == null)
        {
            return NotFound();
        }
        return Ok(container);
    }

    [HttpPost]
    public async Task<IActionResult> CreateContainer([FromBody] Container container)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Containers.Add(container);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContainer), new { id = container.ContainerId }, container);
    }

    [HttpPut("{id}")]
public async Task<IActionResult> UpdateContainer(int id, [FromBody] Container container)
{
    if (id != container.ContainerId)
    {
        return BadRequest("Container ID mismatch.");
    }

    if (!ModelState.IsValid)
    {
        // Log validation errors
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        foreach (var error in errors)
        {
            Console.WriteLine(error.ErrorMessage); // Add a breakpoint or log error messages
        }
        return BadRequest(ModelState);
    }

    // Verify if TruckCompany exists for the given CompanyId
    var truckCompany = await _context.TruckCompanies.FindAsync(container.CompanyId);
    if (truckCompany == null)
    {
        return BadRequest("Invalid Truck Company ID.");
    }

    var existingContainer = await _context.Containers.FindAsync(id);
    if (existingContainer == null)
    {
        return NotFound("Container not found.");
    }

    // Update fields
    existingContainer.ContainerNumber = container.ContainerNumber;
    existingContainer.ChassisNumber = container.ChassisNumber;
    existingContainer.ContainerType = container.ContainerType;
    existingContainer.Capacity = container.Capacity;
    existingContainer.Status = container.Status;
    existingContainer.CompanyId = container.CompanyId;

    // Save changes
    _context.Entry(existingContainer).State = EntityState.Modified;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
        // Log the error details
        Console.WriteLine(ex.Message); // Log exception for further analysis
        return StatusCode(500, "An error occurred while updating the container.");
    }

    return NoContent();
}

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContainer(int id)
    {
        var container = await _context.Containers.FindAsync(id);
        if (container == null)
        {
            return NotFound();
        }

        _context.Containers.Remove(container);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
