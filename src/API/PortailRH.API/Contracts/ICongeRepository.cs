namespace PortailRH.API.Contracts
{
    public interface ICongeRepository : IAsyncRepository<Conge>
    {
        
        Task<IReadOnlyList<Conge>> GetCongesByEmployeeIdAsync(int employeeId);
    }
}