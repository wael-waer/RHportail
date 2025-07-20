namespace PortailRH.API.Features.Employees.GetAllEmployees
{
    public class GetAllEmployeesEndpoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/employees", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllEmployeesQuery());
                return Results.Ok(result);
            })
            .WithName("GetAllEmployees")
            .Produces<List<GetAllEmployeesResult>>(StatusCodes.Status200OK)
            .WithSummary("Get All Employees")
            .WithDescription("Returns a list of all employees.");
        }
    }
}
