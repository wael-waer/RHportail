



namespace PortailRH.API.Repositories
{
    public class EmployeeRepository:RepositoryBase<Employee>, IEmployeeRepository
    {
        private readonly PortailRHContext _dbContext;
        public EmployeeRepository(PortailRHContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<bool> ExistsAsync(Guid id)
        {
            {
                // Si votre Employee.Id est int, cette méthode ne peut pas être implémentée
                // avec Guid. Vous devez soit changer l'interface, soit changer le type d'Id
                throw new NotImplementedException("Employee utilise int comme Id, pas Guid");
            }

        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Employee.FindAsync(id);
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
