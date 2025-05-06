namespace PortailRH.API.Features.Jobs.UpdateJob
{
    public record UpdateJobRequest(int Id, string Title, string Description, string RequiredSkills, string Location,
        string ContractType, decimal Salary, DateTime ApplicationDeadline, DateTime PublicationDate, string Status);

    public record UpdateJobResponse(bool IsSuccess);

    public class UpdateJobEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/jobs", async (UpdateJobRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateJobCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateJobResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateJob")
            .Produces<UpdateJobResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Job")
            .WithDescription("Update an existing job.");
        }
    }
}
