namespace PortailRH.API.Features.Candidatures.DeleteCandidature;

public class DeleteCandidatureEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/candidatures/{id:int}", async (int id, ISender sender) =>
        {
            var command = new DeleteCandidatureCommand(id);
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteCandidatureResult>();
            return Results.Ok(response);
        })
        .WithName("DeleteCandidature")
        .Produces<DeleteCandidatureResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Candidature")
        .WithDescription("Delete a candidature by ID.");
    }
}
