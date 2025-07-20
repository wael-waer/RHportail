

using Microsoft.EntityFrameworkCore;

namespace PortailRH.API.Repositories
{
    public class EmployeeRepository:RepositoryBase<Employee>, IEmployeeRepository
    {
        private readonly PortailRHContext _dbContext;
        public EmployeeRepository(PortailRHContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<Employee?> GetByNumeroIdentificationAsync(string numeroIdentification)
        {
            return await _dbContext.Employee
                .FirstOrDefaultAsync(e => e.NumeroIdentification == numeroIdentification);
        }
        public async Task<SuiviConge?> GetSuiviCongeAsync(int employeeId, int annee)
        {
            return await _dbContext.Set<SuiviConge>()
                .FirstOrDefaultAsync(sc => sc.EmployeeId == employeeId && sc.Annee == annee && sc.Actif);
        }

        public async Task UpdateSuiviCongeAsync(SuiviConge suivi)
        {
            _dbContext.Set<SuiviConge>().Update(suivi);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddSuiviCongeAsync(SuiviConge suiviConge)
        {
            await _dbContext.Set<SuiviConge>().AddAsync(suiviConge);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<SuiviConge>> GetAllSuivisCongeByEmployeeIdAsync(int employeeId)
        {
            return await _dbContext.Set<SuiviConge>()
                .Where(sc => sc.EmployeeId == employeeId)
                .ToListAsync();
        }
        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _dbContext.Employee
                .FirstOrDefaultAsync(e => e.Email == email);
        }




    }

}
