using System.Collections.Generic;
using System.Threading.Tasks;

public interface IContainerService
{
    Task<IEnumerable<Container>> GetContainersAsync();
    Task<Container?> GetContainerByIdAsync(int id);
    Task<Container> CreateContainerAsync(Container container);
    Task UpdateContainerAsync(int id, Container container);
    Task DeleteContainerAsync(int id);
}
