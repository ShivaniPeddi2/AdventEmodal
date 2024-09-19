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
    public async Task<IActionResult> GetTruckCompanies()
    {
        var companies = await _truckCompanyService.GetTruckCompaniesAsync();
        return Ok(companies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTruckCompany(int id)
    {
        try
        {
            var company = await _truckCompanyService.GetTruckCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return Ok(company);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateTruckCompany([FromBody] TruckCompany company)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdCompany = await _truckCompanyService.CreateTruckCompanyAsync(company);
        return CreatedAtAction(nameof(GetTruckCompany), new { id = createdCompany.CompanyId }, createdCompany);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTruckCompany(int id, [FromBody] TruckCompany company)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _truckCompanyService.UpdateTruckCompanyAsync(id, company);
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

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTruckCompany(int id)
    {
        try
        {
            await _truckCompanyService.DeleteTruckCompanyAsync(id);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
