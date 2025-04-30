
namespace PortailRH.API.Features.Conges.CreateConge
{
    public record CreateCongeRequest(string TypeConge, DateTime DateDebut, DateTime DateFin, string Statut, string Motif, int IdEmploye);
    public record CreateCongeResponse(int Id);

    public class CreateCongeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/conges", async (CreateCongeRequest request, ISender sender) =>
            {

                var command = request.Adapt<CreateCongeCommand>();


                var result = await sender.Send(command);


                var response = result.Adapt<CreateCongeResponse>();


                return Results.Created($"/api/conges/{response.Id}", response);
            })
            .WithName("CreateConge")
            .Produces<CreateCongeResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Conge")
            .WithDescription("Create a new Conge record");
        }
    }
}
