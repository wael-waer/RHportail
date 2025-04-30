
namespace PortailRH.API.Features.Projects.CreateProject
{
    public record CreateProjectRequest(string Type, string Title, string Priority, DateTime StartDate, DateTime EndDate);
    public record CreateProjectResponse(int Id);

    public class CreateProjectEndpoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/projects", async (CreateProjectRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProjectCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateProjectResponse>();
                return Results.Created($"/api/projects/{response.Id}", response);
            })
            .WithName("CreateProject")
            .Produces<CreateProjectResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Project")
            .WithDescription("Create a new project.");
        }
    }
}

