namespace PortailRH.API.Contracts
{
    public interface ISuiviCongeRepository
    {
        Task<SuiviConge?> GetSuiviCongeAsync(int employeeId, int annee);
        Task AddSuiviCongeAsync(SuiviConge suiviConge);
        Task UpdateSuiviCongeAsync(SuiviConge suiviConge);
        Task<List<SuiviConge>> GetAllSuivisCongeByEmployeeIdAsync(int employeeId);
    }
}   