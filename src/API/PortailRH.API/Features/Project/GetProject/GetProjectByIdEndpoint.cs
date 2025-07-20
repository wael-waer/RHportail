namespace PortailRH.API.Features.Projects.GetAllProjects
{
    public class GetAllProjectsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/projects", async (ISender sender) =>
            {
                var query = new GetAllProjectsQuery();
                var result = await sender.Send(query);
                return Results.Ok(result);
            })
            .WithName("GetAllProjects")
            .Produces<List<GetAllProjectsResult>>(StatusCodes.Status200OK)
            .WithSummary("Get All Projects")
            .WithDescription("Retrieve all available projects.");
        }
    }
}
