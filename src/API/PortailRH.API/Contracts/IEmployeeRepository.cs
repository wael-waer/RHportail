


namespace PortailRH.API.Contracts
{
    public interface IEmployeeRepository:IAsyncRepository<Employee>
    {
        Task<Employee?> GetByNumeroIdentificationAsync(string numeroIdentification);
        Task<List<SuiviConge>> GetAllSuivisCongeByEmployeeIdAsync(int employeeId);

        Task<SuiviConge?> GetSuiviCongeAsync(int employeeId, int annee);
        Task UpdateSuiviCongeAsync(SuiviConge suivi);
        Task AddSuiviCongeAsync(SuiviConge suiviConge);
        Task<Employee> GetByEmailAsync(string email);

        Task<bool> ExistsAsync(Guid id);
        Task<Employee?> GetByIdAsync(Guid id);


    }
}
    