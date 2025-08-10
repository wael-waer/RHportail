
namespace PortailRH.API.Repositories
{

    public class ContratRepository : RepositoryBase<Contrat>, IContratRepository
    {
        private readonly PortailRHContext _dbContext;

        public ContratRepository(PortailRHContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Contrat>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _dbContext.Contrats
                .Where(c => c.EmployeeId == employeeId)
                .ToListAsync();
        }
    }
}
