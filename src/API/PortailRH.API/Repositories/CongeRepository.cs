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
    }
}
