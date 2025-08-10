namespace PortailRH.API.Features.Employees.GetAllEmployees
{
    public class GetAllEmployeesEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // Get All Employees
            app.MapGet("/api/employees", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllEmployeesQuery());
                return Results.Ok(result);
            })
            .WithName("GetAllEmployees")
            .Produces<List<GetEmployeeResult>>(StatusCodes.Status200OK)
            .WithSummary("Get All Employees")
            .WithDescription("Returns a list of all employees.");

            // Get Employee by Id
            app.MapGet("/api/employees/{id:int}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetEmployeeByIdQuery(id));
                return result is not null ? Results.Ok(result) : Results.NotFound();
            })
            .WithName("GetEmployeeById")
            .Produces<GetEmployeeResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Get Employee by ID")
            .WithDescription("Returns a specific employee by ID.");
        }
    }
}
