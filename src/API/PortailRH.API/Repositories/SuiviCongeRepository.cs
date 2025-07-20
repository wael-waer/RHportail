


namespace PortailRH.API.Repositories
{
    public class SuiviCongeRepository : RepositoryBase<SuiviConge>, ISuiviCongeRepository
    {
        public SuiviCongeRepository(PortailRHContext dbContext) : base(dbContext)
        {
        }

        public async Task<SuiviConge?> GetSuiviCongeAsync(int employeeId, int annee)
        {
            return await _dbContext.Set<SuiviConge>()
                .FirstOrDefaultAsync(sc => sc.EmployeeId == employeeId && sc.Annee == annee && sc.Actif);
        }

        public async Task AddSuiviCongeAsync(SuiviConge suiviConge)
        {
            await AddAsync(suiviConge);
        }

        public async Task UpdateSuiviCongeAsync(SuiviConge suiviConge)
        {
            _dbContext.Set<SuiviConge>().Update(suiviConge);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<SuiviConge>> GetAllSuivisCongeByEmployeeIdAsync(int employeeId)
        {
            return await _dbContext.Set<SuiviConge>()
                .Where(sc => sc.EmployeeId == employeeId)
                .ToListAsync();
        }
    }
}
