namespace PortailRH.API.Features.Conges.UpdateConge
{
    public record UpdateCongeRequest(int Id, string TypeConge, DateTime DateDebut, DateTime DateFin, string Statut, string Motif, int EmployeeId);
    public record UpdateCongeResponse(bool IsSuccess);

    public class UpdateCongeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/conges/{id:int}", async (int Id, UpdateCongeRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateCongeCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateCongeResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateConge")
            .Produces<UpdateCongeResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Conge")
            .WithDescription("Update an existing conge.");
        }
    }
}
