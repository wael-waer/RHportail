namespace PortailRH.API.Contracts
{
    public interface IPayslipRepository : IAsyncRepository<Payslip>
    {

        Task<List<Payslip>> GetAllAsync();
        Task<List<Payslip>> GetPayslipsByEmployeeIdAsync(int employeeId);
      
        Task<Payslip?> GetLatestPayslipForEmployeeAsync(int employeeId);
        Task<List<Payslip>> GetPayslipsByPaymentPolicyIdAsync(int policyId);
        Task AddOrUpdatePayslipAsync(Payslip payslip);

    }
}