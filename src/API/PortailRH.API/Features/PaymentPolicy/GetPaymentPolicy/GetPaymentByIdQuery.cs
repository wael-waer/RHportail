namespace PortailRH.API.Features.PaymentPolicies.GetPaymentPolicyById
{
    public record GetPaymentPolicyByIdQuery(int Id) : IQuery<GetPaymentPolicyByIdResult>;

    public record GetPaymentPolicyByIdResult(
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
    );

    public class GetPaymentPolicyByIdQueryHandler(IPaymentPolicyRepository repository)
        : IQueryHandler<GetPaymentPolicyByIdQuery, GetPaymentPolicyByIdResult>
    {
        public async Task<GetPaymentPolicyByIdResult> Handle(GetPaymentPolicyByIdQuery query, CancellationToken cancellationToken)
        {
            var policy = await repository.GetByIdAsync(query.Id);
            if (policy is null)
                throw new NotFoundException("PaymentPolicy", query.Id);

            return policy.Adapt<GetPaymentPolicyByIdResult>();
        }
    }
}
