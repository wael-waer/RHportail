

namespace PortailRH.API.Repositories
{
    public class EmployeeRepository:RepositoryBase<Employee>, IEmployeeRepository
    {
        private readonly PortailRHContext _dbContext;
        public EmployeeRepository(PortailRHContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

     

        public async Task<IReadOnlyList<Employee>> GetEmployeesByPosteAsync(string Poste)
        {
            return await _dbContext.Employees.Where(e => e.Poste == Poste).ToListAsync();
        }
    }
   
}
