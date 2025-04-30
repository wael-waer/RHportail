namespace PortailRH.API.Features.Candidatures.CreateCandidature
{
    public record CreateCandidatureRequest(string Nom, string Prenom, string Email, string PosteSouhaite);
    public record CreateCandidatureResponse(int Id);

    public class CreateCandidatureEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/candidatures", async (CreateCandidatureRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateCandidatureCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateCandidatureResponse>();
                return Results.Created($"/api/candidatures/{response.Id}", response);
            })
            .WithName("CreateCandidature")
            .Produces<CreateCandidatureResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Candidature")
            .WithDescription("Create a new job candidature.");
        }
    }
}
