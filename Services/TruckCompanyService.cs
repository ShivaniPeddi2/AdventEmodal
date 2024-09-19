using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TruckCompanyService : ITruckCompanyService
{
    private readonly ApplicationDbContext _context;

    public TruckCompanyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TruckCompany>> GetTruckCompaniesAsync()
    {
        return await _context.TruckCompanies
            .Include(c => c.Drivers)
            .Include(c => c.Appointments)
            .Include(c => c.Containers)
            .ToListAsync();
    }

    public async Task<TruckCompany> GetTruckCompanyByIdAsync(int id)
    {
        return await _context.TruckCompanies
            .Include(c => c.Drivers)
            .Include(c => c.Appointments)
            .Include(c => c.Containers)
            .FirstOrDefaultAsync(c => c.CompanyId == id);
    }

    public async Task<TruckCompany> CreateTruckCompanyAsync(TruckCompany company)
    {
        _context.TruckCompanies.Add(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task UpdateTruckCompanyAsync(int id, TruckCompany company)
    {
        if (id != company.CompanyId)
        {
            throw new ArgumentException("Truck Company ID mismatch.");
        }

        var existingCompany = await _context.TruckCompanies.FindAsync(id);
        if (existingCompany == null)
        {
            throw new KeyNotFoundException("Truck Company not found.");
        }

        existingCompany.Name = company.Name;
        existingCompany.Address = company.Address;
        existingCompany.ContactNumber = company.ContactNumber;
        existingCompany.RegisteredDate = company.RegisteredDate;
        existingCompany.Email = company.Email;
        existingCompany.Website = company.Website;

        _context.Entry(existingCompany).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTruckCompanyAsync(int id)
    {
        var company = await _context.TruckCompanies.FindAsync(id);
        if (company == null)
        {
            throw new KeyNotFoundException("Truck Company not found.");
        }

        _context.TruckCompanies.Remove(company);
        await _context.SaveChangesAsync();
    }
}
