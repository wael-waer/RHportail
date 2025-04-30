namespace PortailRH.API.Features.Candidatures.UpdateCandidature
{

    public record UpdateCandidatureRequest(int Id, string Nom, string Prenom, string Email, string PosteSouhaite);
    public record UpdateCandidatureResponse(bool IsSuccess);

    public class UpdateCandidatureEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/candidatures/{id:int}", async (int id, UpdateCandidatureRequest request, ISender sender) =>
            {
                if (id != request.Id)
                    return Results.BadRequest();

                var command = request.Adapt<UpdateCandidatureCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateCandidatureResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateCandidature")
            .Produces<UpdateCandidatureResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Candidature")
            .WithDescription("Update an existing candidature.");
        }
    }
}
