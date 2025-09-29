using Microsoft.AspNetCore.Mvc;

namespace PortailRH.API.Features.Equipes.GetAllEquipes
{
    // 🔹 Query pour récupérer toutes les équipes
    public record GetAllEquipesQuery : IQuery<List<GetEquipeResult>>;

    // 🔹 Résultat pour une seule équipe
    public record GetEquipeResult(
        Guid Id,
        string Nom,
        string? Description,
        DateTime DateCreation,
        int ResponsableId,
        List<int> EmployeeIds
    );

    // 🔹 Handler
    public class GetAllEquipesQueryHandler(IEquipeRepository equipeRepository)
        : IQueryHandler<GetAllEquipesQuery, List<GetEquipeResult>>
    {
        public async Task<List<GetEquipeResult>> Handle(GetAllEquipesQuery query, CancellationToken cancellationToken)
        {
            var equipes = await equipeRepository.GetAllWithEmployeesAsync();

            return equipes.Select(e => new GetEquipeResult(
                e.Id,
                e.Nom,
                e.Description,
                e.DateCreation,
                e.ResponsableId,
                e.Employees.Select(emp => emp.Id).ToList()
            )).ToList();
        }
    }

    // 🔹 Endpoint
    public class GetAllEquipesEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/equipes", async (ISender sender) =>
            {
                var result = await sender.Send(new GetAllEquipesQuery());
                return Results.Ok(result);
            })
            .WithName("GetAllEquipes")
            .Produces<List<GetEquipeResult>>(StatusCodes.Status200OK);
        }
    }
}
