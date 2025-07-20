namespace PortailRH.API.Features.Candidatures.GetAllCandidatures
{
    public class GetAllCandidaturesEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {   
            app.MapGet("/api/candidatures", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllCandidaturesQuery());
                return Results.Ok(result);
            })
            .WithName("GetAllCandidatures")
            .Produces<List<GetAllCandidaturesResult>>(StatusCodes.Status200OK)
            .WithSummary("Get All Candidatures")
            .WithDescription("Retrieve the list of all job candidatures.");
        }
    }
}
