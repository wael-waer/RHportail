namespace PortailRH.API.Features.Employees.CreateEmployee
{
    public record CreateEmployeeRequest(string LastName, string FirstName, string Email, DateTime BirthDate, string Poste);
    public record CreateEmployeeResponse(int Id);

    public class CreateEmployeeEndpoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/employees", async (CreateEmployeeRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateEmployeeCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateEmployeeResponse>();
                return Results.Created($"/api/employees/{response.Id}", response);
            })
            .WithName("CreateEmployee")
            .Produces<CreateEmployeeResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Employee")
            .WithDescription("Create Employee");
        }
    }
}
