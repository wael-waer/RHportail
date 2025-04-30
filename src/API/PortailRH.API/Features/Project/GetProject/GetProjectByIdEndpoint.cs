namespace PortailRH.API.Features.Projects.GetProjectById
{
    public class GetProjectByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/projects/{id:int}", async (int id, ISender sender) =>
            {
                var query = new GetProjectByIdQuery(id);
                var result = await sender.Send(query);
                return Results.Ok(result);
            })
            .WithName("GetProjectById")
            .Produces<GetProjectByIdResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Project by Id")
            .WithDescription("Retrieve a project by its unique identifier.");
        }
    }
}
