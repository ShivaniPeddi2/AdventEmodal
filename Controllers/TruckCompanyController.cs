using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TruckCompanyController : ControllerBase
{
    private readonly ITruckCompanyService _truckCompanyService;

    public TruckCompanyController(ITruckCompanyService truckCompanyService)
    {
        _truckCompanyService = truckCompanyService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTruckCompaniesAsync()
    {
        var companies = await _truckCompanyService.GetTruckCompaniesAsync();
        return Ok(companies);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTruckCompanyByIdAsync(int id)
    {
        var company = await _truckCompanyService.GetTruckCompanyByIdAsync(id);
        if (company == null)
        {
            return NotFound();
        }
        return Ok(company);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateTruckCompanyAsync([FromBody] TruckCompany company)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdCompany = await _truckCompanyService.CreateTruckCompanyAsync(company);
        return CreatedAtAction(nameof(GetTruckCompanyByIdAsync), new { id = createdCompany.CompanyId }, createdCompany);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateTruckCompanyAsync(int id, [FromBody] TruckCompany company)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _truckCompanyService.UpdateTruckCompanyAsync(id, company);
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
        //     return StatusCode(500, "An error occurred while updating the truck company.");
        // }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTruckCompanyAsync(int id)
    {
        try
        {
            await _truckCompanyService.DeleteTruckCompanyAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
