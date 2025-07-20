
namespace PortailRH.API.Repositories
{
    public class AdminRepository : RepositoryBase<Admin>, IAdminRepository
    {
        private readonly PortailRHContext _context;

        public AdminRepository(PortailRHContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}
