using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TruckCompanyController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TruckCompanyController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTruckCompanies()
    {
        var companies = await _context.TruckCompanies.ToListAsync();
        return Ok(companies);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTruckCompany(int id)
    {
        var company = await _context.TruckCompanies.FindAsync(id);
        if (company == null)
        {
            return NotFound();
        }
        return Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTruckCompany([FromBody] TruckCompany company)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.TruckCompanies.Add(company);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTruckCompany), new { id = company.CompanyId }, company);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTruckCompany(int id, [FromBody] TruckCompany company)
    {
        if (id != company.CompanyId)
        {
            return BadRequest();
        }

        var existingCompany = await _context.TruckCompanies.FindAsync(id);
        if (existingCompany == null)
        {
            return NotFound();
        }

        existingCompany.Name = company.Name;
        existingCompany.Address = company.Address;
        existingCompany.ContactNumber = company.ContactNumber;
        existingCompany.RegisteredDate = company.RegisteredDate;
        existingCompany.Email = company.Email;
        existingCompany.Website = company.Website;

        _context.Entry(existingCompany).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTruckCompany(int id)
    {
        var company = await _context.TruckCompanies.FindAsync(id);
        if (company == null)
        {
            return NotFound();
        }

        _context.TruckCompanies.Remove(company);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
