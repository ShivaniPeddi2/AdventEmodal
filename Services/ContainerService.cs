using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ContainerService : IContainerService
{
    private readonly ApplicationDbContext _context;

    public ContainerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Container>> GetContainersAsync()
    {
        return await _context.Containers.ToListAsync();
    }

    public async Task<Container?> GetContainerByIdAsync(int id)
    {
        return await _context.Containers.FindAsync(id);
    }

    public async Task<Container> CreateContainerAsync(Container container)
    {
        _context.Containers.Add(container);
        await _context.SaveChangesAsync();
        return container;
    }

    public async Task UpdateContainerAsync(int id, Container container)
    {
        if (id != container.ContainerId)
        {
            throw new ArgumentException("Container ID mismatch.");
        }

        var truckCompany = await _context.TruckCompanies.FindAsync(container.CompanyId);
        if (truckCompany == null)
        {
            throw new ArgumentException("Invalid Truck Company ID.");
        }

        var existingContainer = await _context.Containers.FindAsync(id);
        if (existingContainer == null)
        {
            throw new KeyNotFoundException("Container not found.");
        }

        existingContainer.ContainerNumber = container.ContainerNumber;
        existingContainer.ChassisNumber = container.ChassisNumber;
        existingContainer.ContainerType = container.ContainerType;
        existingContainer.Capacity = container.Capacity;
        existingContainer.Status = container.Status;
        existingContainer.CompanyId = container.CompanyId;

        _context.Entry(existingContainer).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteContainerAsync(int id)
    {
        var container = await _context.Containers.FindAsync(id);
        if (container == null)
        {
            throw new KeyNotFoundException("Container not found.");
        }

        _context.Containers.Remove(container);
        await _context.SaveChangesAsync();
    }
}
