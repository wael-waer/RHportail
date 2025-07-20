namespace PortailRH.API.Features.Conges.GetCongeStatusById
{
    public class GetCongeStatusByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/conges/{id:int}/status", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetCongeStatusByIdQuery(id));
                return Results.Ok(result);
            })
            .WithName("GetCongeStatusById")
            .Produces<GetCongeStatusByIdResult>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Statut d'un congé")
            .WithDescription("Récupère uniquement le statut d'un congé spécifique.");
        }
    }
}