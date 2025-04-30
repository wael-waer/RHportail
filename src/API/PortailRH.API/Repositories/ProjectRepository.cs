

namespace PortailRH.API.Repositories
{
    public class ProjectRepository:RepositoryBase<Project>, IProjectRepository
    {
        private readonly PortailRHContext _dbContext;

        public ProjectRepository(PortailRHContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}