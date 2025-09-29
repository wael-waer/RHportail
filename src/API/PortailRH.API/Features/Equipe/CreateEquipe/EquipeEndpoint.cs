using Microsoft.AspNetCore.Mvc;

namespace PortailRH.API.Features.Equipes.CreateEquipe
{
    public record CreerEquipeRequest(
        string Nom,
        string? Description,
        List<int> EmployeeIds,
        int ResponsableId
    );

    public record CreerEquipeResponse(bool Success, string Message, Guid? EquipeId = null);

    public record AssignerEmployesRequest(List<int> EmployeeIds);

    public record AssignerEmployesResponse(bool Success, string Message);

    public class EquipesEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // Endpoint pour créer une équipe
            app.MapPost("/api/equipes", async (CreerEquipeRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreerEquipeCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreerEquipeResponse>();

                return result.Success
                    ? Results.Created($"/api/equipes/{response.EquipeId}", response)
                    : Results.BadRequest(response);
            })
            .WithName("CreerEquipe")
            .Produces<CreerEquipeResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Créer une nouvelle équipe")
            .WithDescription("Crée une nouvelle équipe avec des employés assignés.");

            // Endpoint pour assigner des employés à une équipe existante
            app.MapPost("/api/equipes/{equipeId:guid}/assigner-employes", async (
                [FromRoute] Guid equipeId,
                AssignerEmployesRequest request,
                ISender sender) =>
            {
                var command = new AssignerEmployesCommand(equipeId, request.EmployeeIds);
                var result = await sender.Send(command);
                var response = result.Adapt<AssignerEmployesResponse>();

                return result.Success
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            })
            .WithName("AssignerEmployes")
            .Produces<AssignerEmployesResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Assigner des employés à une équipe existante")
            .WithDescription("Assigne une liste d'employés à l'équipe spécifiée par son ID.");
        }
    }
}
