namespace PortailRH.API.Features.PaymentPolicies.CreatePaymentPolicy
{
    public record CreatePaymentPolicyRequest(
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

    public record CreatePaymentPolicyResponse(int Id);
    public class CreatePaymentPolicyEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/payment-policies", async (CreatePaymentPolicyRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreatePaymentPolicyCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreatePaymentPolicyResponse>();
                return Results.Created($"/api/payment-policies/{response.Id}", response);
            })
            .WithName("CreatePaymentPolicy")
            .Produces<CreatePaymentPolicyResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithSummary("Create Payment Policy")
            .WithDescription("Create a new payment policy to be used for payslip generation.");
        }
    }
}
