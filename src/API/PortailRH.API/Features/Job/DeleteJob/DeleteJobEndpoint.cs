namespace PortailRH.API.Features.Jobs.DeleteJob
{
    public class DeleteJobEndpoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/jobs/{id:int}", async (int id, ISender sender) =>
            {
                var command = new DeleteJobCommand(id);
                var result = await sender.Send(command);

                if (!result.IsSuccess)
                    return Results.NotFound();

                return Results.NoContent();
            })
            .WithName("DeleteJob")
            .Produces<DeleteJobResult>(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Job")
            .WithDescription("Delete a job by ID.");
        }
    }
}
