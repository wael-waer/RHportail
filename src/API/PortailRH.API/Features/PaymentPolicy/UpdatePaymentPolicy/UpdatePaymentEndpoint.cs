namespace PortailRH.API.Features.PaymentPolicies.UpdatePaymentPolicy
{
    public record UpdatePaymentPolicyRequest(
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

    public record UpdatePaymentPolicyResponse(bool IsSuccess);
    public class UpdatePaymentPolicyEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/paymentpolicies/{id:int}", async (int id, UpdatePaymentPolicyCommand command, ISender sender) =>
            {
                if (id != command.Id)
                    return Results.BadRequest("ID mismatch.");

                var result = await sender.Send(command);
                return Results.Ok(result);
            })
            .WithName("UpdatePaymentPolicy")
            .Produces<UpdatePaymentPolicyResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Payment Policy")
            .WithDescription("Update an existing payment policy by ID.");
        }
    }
}