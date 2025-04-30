
namespace PortailRH.API.Features.Employees.DeleteEmployee
{
    public class DeleteEmployeeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/employees/{id:int}", async (int id, ISender sender) =>
            {
                var command = new DeleteEmployeeCommand(id);
                var result = await sender.Send(command);
                if (!result.IsSuccess)
                {
                    return Results.NotFound($"Employee with Id {id} not found");
                }

                return Results.NoContent();
            })
            .WithName("DeleteEmployee")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Employee")
            .WithDescription("Delete an employee by ID");
        }
    }
}
