
namespace PortailRH.API.Repositories
{
    public class PayslipRepository : RepositoryBase<Payslip>, IPayslipRepository
    {
        public PayslipRepository(PortailRHContext dbContext) : base(dbContext) { }

        public async Task<List<Payslip>> GetPayslipsByEmployeeIdAsync(int employeeId)
        {
            return await _dbContext.Payslips
                .Where(p => p.EmployeeId == employeeId)
                .OrderByDescending(p => p.GeneratedAt)
                .ToListAsync();
        }

        public async Task<Payslip?> GetLatestPayslipForEmployeeAsync(int employeeId)
        {
            return await _dbContext.Payslips
                .Where(p => p.EmployeeId == employeeId)
                .OrderByDescending(p => p.GeneratedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Payslip>> GetPayslipsByPaymentPolicyIdAsync(int policyId)
        {
            return await _dbContext.Payslips
                .Where(p => p.PaymentPolicyId == policyId)
                .ToListAsync();
        }
    }
}