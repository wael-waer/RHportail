using Microsoft.EntityFrameworkCore;

namespace PortailRH.API.Repositories
{
    public class JobRepository : RepositoryBase<Job>, IJobRepository
    {
        private readonly PortailRHContext _dbContext;

        public JobRepository(PortailRHContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IReadOnlyList<Job>> GetByIdAsync(int id)
        {
            return await _dbContext.Jobs
                .Include(j => j.Applicants)
                .Where(j => j.Id == id)
                .ToListAsync();
        }
        
    }
}
    