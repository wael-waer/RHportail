namespace PortailRH.API.Repositories
{
    public class CandidatureRepository : RepositoryBase<Candidature>, ICandidatureRepository
    {
        private readonly PortailRHContext _dbContext;

        public CandidatureRepository(PortailRHContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
       
    }
}
