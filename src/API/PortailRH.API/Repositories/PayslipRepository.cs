namespace PortailRH.API.Repositories
{
    public class PayslipRepository : RepositoryBase<Payslip>, IPayslipRepository
    {
        public PayslipRepository(PortailRHContext dbContext) : base(dbContext) { }

       
        public async Task<List<Payslip>> GetAllAsync()
        {
            return await _dbContext.Payslips
                .Include(p => p.Employee)   // Include related Employee
                .ToListAsync();
        }

        public async Task<List<Payslip>> GetPayslipsByEmployeeIdAsync(int employeeId)
        {
            return await _dbContext.Payslips
                .Where(p => p.EmployeeId == employeeId)
                .Include(p => p.Employee)
                .OrderByDescending(p => p.GeneratedAt)
                .ToListAsync();
        }

        public async Task<Payslip?> GetLatestPayslipForEmployeeAsync(int employeeId)
        {
            return await _dbContext.Payslips
                .Where(p => p.EmployeeId == employeeId)
                .Include(p => p.Employee)
                .OrderByDescending(p => p.GeneratedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Payslip>> GetPayslipsByPaymentPolicyIdAsync(int policyId)
        {
            return await _dbContext.Payslips
                .Where(p => p.PaymentPolicyId == policyId)
                .Include(p => p.Employee)
                .ToListAsync();
        }

        public async Task AddOrUpdatePayslipAsync(Payslip payslip)
        {
            var existing = await _dbContext.Payslips
                .FirstOrDefaultAsync(p =>
                    p.EmployeeId == payslip.EmployeeId &&
                    p.PaymentPolicyId == payslip.PaymentPolicyId);

            if (existing != null)
            {
                existing.BasicSalary = payslip.BasicSalary;
                existing.TaxDeduction = payslip.TaxDeduction;
                existing.SocialSecurityDeduction = payslip.SocialSecurityDeduction;
                existing.OtherDeductions = payslip.OtherDeductions;
                existing.NetSalary = payslip.NetSalary;
                existing.GeneratedAt = payslip.GeneratedAt;

                _dbContext.Payslips.Update(existing);
            }
            else
            {
                await _dbContext.Payslips.AddAsync(payslip);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
