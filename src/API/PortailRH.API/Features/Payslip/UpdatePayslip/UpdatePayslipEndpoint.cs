namespace PortailRH.API.Features.Employees.UpdateSalary
{
    public record UpdateSalaryRequest(decimal salary);

    public class UpdateSalaryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/employees/{id}/salary", async (int id, UpdateSalaryRequest request, ISender sender) =>
            {
                var command = new UpdateSalaryCommand(id, request.salary);
                await sender.Send(command);
                return Results.Ok();
            })
            .WithName("UpdateEmployeeSalary")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .WithSummary("Update employee salary")
            .WithDescription("Updates the salary of the employee identified by their ID.");
        }
    }
}
