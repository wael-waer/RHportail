namespace PortailRH.API.Features.Projects.DeleteProject
{
    public class DeleteProjectEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/projects/{id:int}", async (int id, ISender sender) =>
            {
                var command = new DeleteProjectCommand(id);
                await sender.Send(command);
                return Results.NoContent();
            })
            .WithName("DeleteProject")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Project")
            .WithDescription("Delete a project by its ID.");
        }
    }
}
