namespace PortailRH.API.Features.PaymentPolicies.UpdatePaymentPolicy
{
    public record UpdatePaymentPolicyCommand(
        int Id,
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

    ) : ICommand<UpdatePaymentPolicyResult>;
    public record UpdatePaymentPolicyResult(bool IsSuccess);
    public class UpdatePaymentPolicyCommandValidator : AbstractValidator<UpdatePaymentPolicyCommand>
    {
        public UpdatePaymentPolicyCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.TaxRate).GreaterThanOrEqualTo(0);
            RuleFor(x => x.SocialSecurityRate).GreaterThanOrEqualTo(0);
            RuleFor(x => x.OtherDeductions).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PaymentDay).InclusiveBetween(1, 31);
            RuleFor(x => x.AllowedDays).GreaterThanOrEqualTo(0);
        }
    }



    public class UpdatePaymentPolicyCommandHandler(IPaymentPolicyRepository repository)
        : ICommandHandler<UpdatePaymentPolicyCommand, UpdatePaymentPolicyResult>
    {
        public async Task<UpdatePaymentPolicyResult> Handle(UpdatePaymentPolicyCommand command, CancellationToken cancellationToken)
        {
            var policy = await repository.GetByIdAsync(command.Id);
            if (policy is null)
            {
                throw new NotFoundException("PaymentPolicy", command.Id);
            }

            policy.TaxRate = command.TaxRate;
            policy.SocialSecurityRate = command.SocialSecurityRate;
            policy.OtherDeductions = command.OtherDeductions;
            policy.PaymentDay = command.PaymentDay;
            policy.ExcessDayPayment = command.ExcessDayPayment;
            policy.AllowedDays = command.AllowedDays;
            policy.SickLeave = command.SickLeave;
            policy.PaidLeave = command.PaidLeave;
            policy.UnpaidLeave = command.UnpaidLeave;
            policy.Bereavement = command.Bereavement;
            policy.PersonalReasons = command.PersonalReasons;
            policy.Maternity = command.Maternity;
            policy.Paternity = command.Paternity;
            policy.RTT = command.RTT;
            policy.Other = command.Other;

            await repository.UpdateAsync(policy);

            return new UpdatePaymentPolicyResult(true);
        }
    }

}
