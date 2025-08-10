

namespace PortailRH.API.Contracts;

public interface IContratRepository : IAsyncRepository<Contrat>
{
    Task<List<Contrat>> GetByEmployeeIdAsync(int employeeId);
    Task<Contrat> AddAsync(Contrat contrat);
}
