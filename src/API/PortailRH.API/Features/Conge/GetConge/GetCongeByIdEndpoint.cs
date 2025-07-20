namespace PortailRH.API.Features.Conges.GetCongeById
{
    public class GetAllCongesEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/conges", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllCongesQuery());
                return Results.Ok(result);
            })
            .WithName("GetAllConges")
            .Produces<List<Conge>>(StatusCodes.Status200OK)
            .WithSummary("Get All Congés")
            .WithDescription("Récupère toutes les demandes de congé.");
        }
    }

}
