
namespace PortailRH.API.Contracts
{
    public interface IEmployeeRepository:IAsyncRepository<Employee>
    {
        Task<IReadOnlyList<Employee>> GetEmployeesByPosteAsync(string Poste);
    }
}
