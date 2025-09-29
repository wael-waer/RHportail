using Microsoft.EntityFrameworkCore;
using PortailRH.API.Contracts;
using PortailRH.API.Models.DataBase;

namespace PortailRH.API.Repositories
{
    public class EquipeRepository : RepositoryBase<Equipe>, IEquipeRepository
    {
        private readonly PortailRHContext _dbContext;

        public EquipeRepository(PortailRHContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Equipe?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Equipes
                .Include(e => e.Employees)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Equipe?> GetByIdWithEmployeesAsync(Guid id) => await GetByIdAsync(id);

        public async Task<IEnumerable<Equipe>> GetAllWithEmployeesAsync()
        {
            return await _dbContext.Equipes
                .Include(e => e.Employees)
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string nom)
        {
            return await _dbContext.Equipes.AnyAsync(e => e.Nom.ToLower() == nom.ToLower());
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbContext.Equipes.AnyAsync(e => e.Id == id);
        }

        public async Task<bool> EmployeeIsInEquipeAsync(int employeeId, Guid equipeId)
        {
            return await _dbContext.Employee
                .AnyAsync(emp => emp.Id == employeeId && emp.EquipeId == equipeId);
        }

        public async Task<Equipe> CreateEquipeWithEmployeesAsync(Equipe equipe, IEnumerable<int> employeeIds)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await _dbContext.Equipes.AddAsync(equipe);
                await _dbContext.SaveChangesAsync();

                if (employeeIds != null && employeeIds.Any())
                {
                    var idsList = employeeIds.ToList();
                    var employees = await _dbContext.Employee
                        .Where(emp => idsList.Contains(emp.Id))
                        .ToListAsync();

                    foreach (var emp in employees)
                        emp.EquipeId = equipe.Id;

                    await _dbContext.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return equipe;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateEquipeWithEmployeesAsync(Equipe equipe, IEnumerable<int> employeeIds)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                _dbContext.Equipes.Update(equipe);
                await _dbContext.SaveChangesAsync();

                var existingEquipe = await _dbContext.Equipes
                    .Include(e => e.Employees)
                    .FirstOrDefaultAsync(e => e.Id == equipe.Id);

                if (existingEquipe == null)
                    throw new KeyNotFoundException("Équipe introuvable.");

                foreach (var emp in existingEquipe.Employees)
                    emp.EquipeId = null;

                existingEquipe.Employees.Clear();

                if (employeeIds != null && employeeIds.Any())
                {
                    var idsList = employeeIds.ToList();
                    var employees = await _dbContext.Employee
                        .Where(emp => idsList.Contains(emp.Id))
                        .ToListAsync();

                    foreach (var emp in employees)
                        emp.EquipeId = equipe.Id;

                    existingEquipe.Employees = employees;
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task AssignEmployeesToEquipeAsync(Guid equipeId, IEnumerable<int> employeeIds)
        {
            var equipe = await _dbContext.Equipes
                .Include(e => e.Employees)
                .FirstOrDefaultAsync(e => e.Id == equipeId);

            if (equipe == null)
                throw new KeyNotFoundException($"Équipe {equipeId} introuvable.");

            foreach (var emp in equipe.Employees)
                emp.EquipeId = null;

            equipe.Employees.Clear();

            if (employeeIds != null && employeeIds.Any())
            {
                var idsList = employeeIds.ToList();
                var employees = await _dbContext.Employee
                    .Where(emp => idsList.Contains(emp.Id))
                    .ToListAsync();

                foreach (var emp in employees)
                    emp.EquipeId = equipeId;

                equipe.Employees = employees;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
