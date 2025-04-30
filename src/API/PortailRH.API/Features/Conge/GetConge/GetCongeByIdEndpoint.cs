namespace PortailRH.API.Features.Conges.GetCongeById
{
    public class GetCongeByIdEndpoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/conges/{id:int}", async (int id, ISender sender) =>
            {
                var query = new GetCongeByIdQuery(id);
                var result = await sender.Send(query);

                if (result == null)
                    return Results.NotFound();

                return Results.Ok(result);
            })
            .WithName("GetCongeById")
            .Produces<GetCongeByIdResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Conge by Id")
            .WithDescription("Retrieve a conge using its ID.");
        }
    }
}
