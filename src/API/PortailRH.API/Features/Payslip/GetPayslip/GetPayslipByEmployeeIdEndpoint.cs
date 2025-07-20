namespace PortailRH.API.Features.Payslips.GetAllPayslips
{
    public record GetAllPayslipsRequest();
    public record GetAllPayslipsResponse(
        int EmployeeId,
        string FullName,
        string Email,
        string Poste,
        int Month,
        int Year,
        decimal BasicSalary,
        decimal TaxDeduction,
        decimal SocialSecurityDeduction,
        decimal OtherDeductions,
        decimal NetSalary,
        string NetSalaryInWords
    );

    public class GetAllPayslipsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/payslips", async (ISender sender) =>
            {
                var query = new GetAllPayslipsQuery();
                var result = await sender.Send(query);
                return Results.Ok(result);
            })
           .WithName("GetAllPayslips")
           .Produces<List<GetAllPayslipsResult>>()
           .ProducesValidationProblem()
          .WithSummary("Get all payslips")
          .WithDescription("Retrieves all payslips for all employees.");
        }
    }
}