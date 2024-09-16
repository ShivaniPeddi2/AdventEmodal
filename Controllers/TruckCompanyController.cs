using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetTruckCompanies()
    {
        if (User.IsInRole("Admin"))
        {
            // Admins can view all truck companies
            var companies = await _context.TruckCompanies.ToListAsync();
            return Ok(companies);
        }
        else
        {
            // Users may not have access to view truck companies unless specific conditions are met
            // For example, you can return a limited list or deny access entirely
            // Here, as an example, users are denied access
            return Forbid();
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTruckCompany(int id)
    {
        var company = await _context.TruckCompanies.FindAsync(id);
        if (company == null)
        {
            return NotFound();
        }

        if (User.IsInRole("Admin"))
        {
            return Ok(company);
        }
        else
        {
            // Regular users cannot access specific truck company details
            return Forbid();
        }
    }

    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Admin")]
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

        _context.Entry(existingCompany).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
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
