namespace PortailRH.API.Features.PaymentPolicies.CreatePaymentPolicy
{
    public record CreatePaymentPolicyCommand(
        decimal TaxRate,
        decimal SocialSecurityRate,
        decimal OtherDeductions,
        int PaymentDay,
        int ExcessDayPayment,
        int AllowedDays,
        int SickLeave,
        int PaidLeave,
        int UnpaidLeave,
        int Bereavement,
        int PersonalReasons,
        int Maternity,
        int Paternity,
        int RTT,
        int Other
    ) : ICommand<CreatePaymentPolicyResult>;

    public record CreatePaymentPolicyResult(int Id);

    public class CreatePaymentPolicyCommandValidator : AbstractValidator<CreatePaymentPolicyCommand>
    {
        public CreatePaymentPolicyCommandValidator()
        {
            RuleFor(x => x.TaxRate).GreaterThanOrEqualTo(0);
            RuleFor(x => x.SocialSecurityRate).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PaymentDay).InclusiveBetween(1, 31);
        }
    }

    public class CreatePaymentPolicyCommandHandler(IPaymentPolicyRepository repository)
        : ICommandHandler<CreatePaymentPolicyCommand, CreatePaymentPolicyResult>
    {
        public async Task<CreatePaymentPolicyResult> Handle(CreatePaymentPolicyCommand command, CancellationToken cancellationToken)
        {
            var policy = command.Adapt<PaymentPolicy>();
            var created = await repository.AddAsync(policy);
            return new CreatePaymentPolicyResult(created.Id);
        }
    }
}
