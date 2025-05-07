namespace PortailRH.API.Features.Payslips.GeneratePayslips
{
    public record GeneratePayslipsRequest(int PaymentPolicyId);
    public record GeneratePayslipsResponse(List<int> PayslipIds);

    public class GeneratePayslipsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/payslips/generate", async (GeneratePayslipsRequest request, ISender sender) =>
            {
                var command = request.Adapt<GeneratePayslipsCommand>();
                var result = await sender.Send(command);
                return Results.Ok(result.Adapt<GeneratePayslipsResponse>());
            })
            .WithName("GeneratePayslips")
            .Produces<GeneratePayslipsResponse>()
            .ProducesValidationProblem()
            .WithSummary("Generate payslips for all employees")
            .WithDescription("Generates payslips for each registered employee based on the selected payment policy.");
        }
    }
}
