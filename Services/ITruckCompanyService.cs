using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITruckCompanyService
{
    Task<IEnumerable<TruckCompany>> GetTruckCompaniesAsync();
    Task<TruckCompany> GetTruckCompanyByIdAsync(int id);
    Task<TruckCompany> CreateTruckCompanyAsync(TruckCompany company);
    Task UpdateTruckCompanyAsync(int id, TruckCompany company);
    Task DeleteTruckCompanyAsync(int id);
}
