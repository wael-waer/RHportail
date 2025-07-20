namespace PortailRH.API.Features.Payslips.ManualAdd
{
    public record ManualPayslipRequest(
        int EmployeeId,
        int PaymentPolicyId,
        decimal BasicSalary
    );

    public record ManualPayslipResponse(int PayslipId);

    public class ManualPayslipEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/payslips/manual", async (ManualPayslipRequest request, ISender sender) =>
            {
                var command = request.Adapt<ManualPayslipCommand>();
                var result = await sender.Send(command);
                return Results.Ok(result.Adapt<ManualPayslipResponse>());
            });
        }
    }

    public record ManualPayslipCommand(int EmployeeId, int PaymentPolicyId, decimal BasicSalary)
        : ICommand<ManualPayslipResult>;

    public record ManualPayslipResult(int PayslipId);

    public class ManualPayslipHandler(
        IPaymentPolicyRepository policyRepo,
        IPayslipRepository payslipRepo)
        : ICommandHandler<ManualPayslipCommand, ManualPayslipResult>
    {
        public async Task<ManualPayslipResult> Handle(ManualPayslipCommand cmd, CancellationToken ct)
        {
            var policy = await policyRepo.GetByIdAsync(cmd.PaymentPolicyId);
            if (policy is null)
                throw new ArgumentException("Policy not found");

            var tax = cmd.BasicSalary * (policy.TaxRate / 100m);
            var social = cmd.BasicSalary * (policy.SocialSecurityRate / 100m);
            var other = policy.OtherDeductions;

            var net = cmd.BasicSalary - tax - social - other;

            var payslip = new Payslip
            {
                EmployeeId = cmd.EmployeeId,
                PaymentPolicyId = cmd.PaymentPolicyId,
                BasicSalary = cmd.BasicSalary,
                TaxDeduction = tax,
                SocialSecurityDeduction = social,
                OtherDeductions = other,
                NetSalary = net,
                GeneratedAt = DateTime.UtcNow
            };

            await payslipRepo.AddAsync(payslip);

            return new ManualPayslipResult(payslip.Id);
        }
    }
}
