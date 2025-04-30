
namespace PortailRH.API.Features.Employees.GetEmployeeById
{
    public class GetEmployeeByIdEndpoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/employees/{id:int}", async (int id, ISender sender) =>
            {
                var query = new GetEmployeeByIdQuery(id);
                var result = await sender.Send(query);

                if (result == null)
                    return Results.NotFound();

                return Results.Ok(result);
            })
            .WithName("GetEmployeeById")
            .Produces<GetEmployeeByIdResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Employee by Id")
            .WithDescription("Retrieve an employee using their ID.");
        }
    }
}
