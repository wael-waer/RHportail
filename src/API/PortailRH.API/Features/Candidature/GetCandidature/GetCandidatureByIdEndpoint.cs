namespace PortailRH.API.Features.Candidatures.GetCandidatureById
{

    public class GetCandidatureByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/candidatures/{id:int}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetCandidatureByIdQuery(id));
                return Results.Ok(result);
            })
            .WithName("GetCandidatureById")
            .Produces<GetCandidatureByIdResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Candidature by Id")
            .WithDescription("Retrieve a candidature using its ID.");
        }
    }
}

