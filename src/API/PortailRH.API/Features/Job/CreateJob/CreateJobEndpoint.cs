
namespace PortailRH.API.Features.Jobs.Createjob
{
    public record CreateJobRequest(
        string Title,
        string Description,
        string RequiredSkills,
        string Location,
        string ContractType,
        decimal Salary,
        DateTime ApplicationDeadline,
        DateTime PublicationDate,
        string Status);

    public record CreateJobResponse(int Id);

    public class CreateJobEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/jobs", async (CreateJobRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateJobCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateJobResponse>();
                return Results.Created($"/api/jobs/{response.Id}", response);
            })
            .WithName("CreateJob")
            .Produces<CreateJobResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Job")
            .WithDescription("Create a new job offer.");
        }
    }
}
