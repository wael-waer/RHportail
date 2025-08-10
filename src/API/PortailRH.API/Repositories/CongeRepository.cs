using Microsoft.EntityFrameworkCore;

namespace PortailRH.API.Repositories
{
    public class CongeRepository : RepositoryBase<Conge>, ICongeRepository
    {
        private readonly PortailRHContext _dbContext;

        public CongeRepository(PortailRHContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IReadOnlyList<Conge>> GetCongesByEmployeeIdAsync(int employeeId)
        {
            return await _dbContext.Conges.Where(c => c.EmployeeId == employeeId)
                .ToListAsync();

        }
        public async Task<bool> ExistsCongeForEmployeeInYearAsync(int employeeId, int year)
        {
            return await _dbContext.Conges.AnyAsync(c =>
                c.EmployeeId == employeeId &&
                (c.DateDebut.Year == year || c.DateFin.Year == year));
        }
        public async Task<Conge?> GetByIdAsync(int id)
        {
            return await _dbContext.Conges.FirstOrDefaultAsync(c => c.Id == id);
        }





    }
}
