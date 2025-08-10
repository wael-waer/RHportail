namespace PortailRH.API.Contracts
{
    public interface ICongeRepository : IAsyncRepository<Conge>
    {
        
        Task<IReadOnlyList<Conge>> GetCongesByEmployeeIdAsync(int employeeId);
        Task<bool> ExistsCongeForEmployeeInYearAsync(int employeeId, int year);
        Task<Conge?> GetByIdAsync(int id);

    }
}