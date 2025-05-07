namespace PortailRH.API.Features.PaymentPolicies.GetPaymentPolicyById
{
    public class GetPaymentPolicyByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/paymentpolicies/{id:int}", async (int id, ISender sender) =>
            {
                var query = new GetPaymentPolicyByIdQuery(id);
                var result = await sender.Send(query);

                return Results.Ok(result);
            })
            .WithName("GetPaymentPolicyById")
            .Produces<GetPaymentPolicyByIdResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Payment Policy by ID")
            .WithDescription("Retrieve a payment policy by its unique ID.");
        }
    }
}