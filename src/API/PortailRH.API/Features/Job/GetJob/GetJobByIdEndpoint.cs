namespace PortailRH.API.Features.Jobs.GetJobById
{
    public class GetJobByIdEndpoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/jobs/{id:int}", async (int id, ISender sender) =>
            {
                var query = new GetJobByIdQuery(id);
                var result = await sender.Send(query);

                if (result == null)
                    return Results.NotFound();

                return Results.Ok(result);
            })
            .WithName("GetJobById")
            .Produces<GetJobByIdResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Job by Id")
            .WithDescription("Retrieve a job using its ID.");
        }
    }
}
