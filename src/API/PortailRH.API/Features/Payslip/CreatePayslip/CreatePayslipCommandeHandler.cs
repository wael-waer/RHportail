
namespace PortailRH.API.Features.Payslips.GeneratePayslips
{
    public record GeneratePayslipsCommand(int PaymentPolicyId) : ICommand<GeneratePayslipsResult>;

    public record GeneratePayslipsResult(List<int> PayslipIds);
    public class GeneratePayslipsCommandHandler(
        IEmployeeRepository employeeRepo,
        IPaymentPolicyRepository policyRepo,
        IPayslipRepository payslipRepo)
        : ICommandHandler<GeneratePayslipsCommand, GeneratePayslipsResult>
    {
        public async Task<GeneratePayslipsResult> Handle(GeneratePayslipsCommand command, CancellationToken cancellationToken)
        {
            var policy = await policyRepo.GetByIdAsync(command.PaymentPolicyId);
            if (policy is null)
                throw new ArgumentException("Policy not found");

            var employees = await employeeRepo.GetAllAsync();
            var payslips = new List<Payslip>();

            foreach (var emp in employees)
            {
                var payslip = new Payslip
                {
                    EmployeeId = emp.Id,
                    PaymentPolicyId = policy.Id,
                    BasicSalary = 20000, // ou une valeur dynamique
                    TaxDeduction = 20000 * policy.TaxRate,
                    SocialSecurityDeduction = 20000 * policy.SocialSecurityRate,
                    OtherDeductions = policy.OtherDeductions,
                    NetSalary = 20000 - (20000 * policy.TaxRate) - (20000 * policy.SocialSecurityRate) - policy.OtherDeductions,
                };
                payslips.Add(payslip);
            }

            foreach (var ps in payslips)
                await payslipRepo.AddAsync(ps);

            return new GeneratePayslipsResult(payslips.Select(p => p.Id).ToList());
        }
    }
}
