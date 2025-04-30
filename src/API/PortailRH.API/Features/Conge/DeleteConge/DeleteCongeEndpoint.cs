namespace PortailRH.API.Features.Conges.DeleteConge
{
    public class DeleteCongeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/conges/{id:int}", async (int id, ISender sender) =>
            {
                var command = new DeleteCongeCommand(id);
                var result = await sender.Send(command);
                if (!result.IsSuccess)
                {
                    return Results.NotFound($"Conge with Id {id} not found");
                }

                return Results.NoContent();
            })
            .WithName("DeleteConge")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Conge")
            .WithDescription("Delete a conge by ID");
        }
    }
}
