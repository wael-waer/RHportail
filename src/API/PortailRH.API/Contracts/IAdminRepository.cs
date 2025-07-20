namespace PortailRH.API.Contracts
{
    public interface IAdminRepository : IAsyncRepository<Admin>
    {
        Task<Admin?> GetByEmailAsync(string email);
    }
}
