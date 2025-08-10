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
                throw new ArgumentException("Payment policy not found");

            var employees = await employeeRepo.GetAllAsync();
            var payslips = new List<Payslip>();

            foreach (var emp in employees)
            {
               
                

                var taxRate = policy.TaxRate / 100m;
                var socialRate = policy.SocialSecurityRate / 100m;

               
                var payslip = new Payslip
                {
                    EmployeeId = emp.Id,
                    PaymentPolicyId = policy.Id,
                    
                    GeneratedAt = DateTime.UtcNow
                };

                payslips.Add(payslip);
            }

            foreach (var ps in payslips)
            {
                await payslipRepo.AddAsync(ps);
            }

            return new GeneratePayslipsResult(payslips.Select(p => p.Id).ToList());
        }
    }
}
