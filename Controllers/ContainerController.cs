using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ContainerController : ControllerBase
{
    private readonly IContainerService _containerService;

    public ContainerController(IContainerService containerService)
    {
        _containerService = containerService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetContainers()
    {
        var containers = await _containerService.GetContainersAsync();
        return Ok(containers);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetContainer(int id)
    {
        try
        {
            var container = await _containerService.GetContainerByIdAsync(id);
            if (container == null)
            {
                return NotFound();
            }
            return Ok(container);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateContainer([FromBody] Container container)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdContainer = await _containerService.CreateContainerAsync(container);
        return CreatedAtAction(nameof(GetContainer), new { id = createdContainer.ContainerId }, createdContainer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContainer(int id, [FromBody] Container container)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _containerService.UpdateContainerAsync(id, container);
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
        //     return StatusCode(500, "An error occurred while updating the container.");
        // }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContainer(int id)
    {
        try
        {
            await _containerService.DeleteContainerAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
