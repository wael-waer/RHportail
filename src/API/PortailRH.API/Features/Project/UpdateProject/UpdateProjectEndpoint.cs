namespace PortailRH.API.Features.Projects.UpdateProject
{
    public record UpdateProjectRequest(int Id, string Type, string Title, string Priority, DateTime StartDate, DateTime EndDate);
    public record UpdateProjectResponse(bool IsSuccess);

    public class UpdateProjectEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/projects/{id:int}", async (int id, UpdateProjectRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProjectCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateProjectResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateProject")
            .Produces<UpdateProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Project")
            .WithDescription("Update an existing project.");
        }
    }
}
