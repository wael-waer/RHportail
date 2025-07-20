namespace PortailRH.API.Features.Jobs.GetAllJobs
{
    public class GetAllJobsEndpoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/jobs", async (ISender sender) =>
            {
                var query = new GetAllJobsQuery();
                var result = await sender.Send(query);

                if (result == null || result.Count == 0)
                    return Results.NotFound();

                return Results.Ok(result);
            })
            .WithName("GetAllJobs")
            .Produces<List<GetAllJobsResult>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get All Jobs")
            .WithDescription("Retrieve all available jobs.");
        }
    }
}
