namespace PortailRH.API.Features.Employees.UpdateEmployee
{
    public record UpdateEmployeeRequest(int Id, string LastName, string FirstName, string Email, DateTime BirthDate, string Poste);
    public record UpdateEmployeeResponse(bool IsSuccess);

    public class UpdateEmployeeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/employees/{id:int}", async ( int Id, UpdateEmployeeRequest request, ISender sender) =>
            {
                
                var command = request.Adapt<UpdateEmployeeCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateEmployeeResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateEmployee")
            .Produces<UpdateEmployeeResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Employee")
            .WithDescription("Update an existing employee.");
        }
    }
}
