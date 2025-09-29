namespace PortailRH.API.Features.Conges.UpdateConge
{
    public record UpdateCongeRequest(string TypeConge, DateTime DateDebut, DateTime DateFin, string Statut, string Motif, int EmployeeId);
    public record UpdateCongeResponse(bool IsSuccess);

    public class UpdateCongeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/conges/{id:int}", async (int id, UpdateCongeRequest request, ISender sender) =>
            {
                // On adapte la requête en commande en ajoutant l'Id depuis l'URL
                var command = request.Adapt<UpdateCongeCommand>() with { Id = id };

                var result = await sender.Send(command);
                var response = result.Adapt<UpdateCongeResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateConge")
            .Produces<UpdateCongeResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Conge")
            .WithDescription("Update an existing conge and trigger real-time notification.");
        }
    }
}
