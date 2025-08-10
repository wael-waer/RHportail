namespace PortailRH.API.Features.Conges.GetCongesByEmployeeId;

public class GetCongesByEmployeeIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/employes/{employeeId:int}/conges", async (
            int employeeId,
            ICongeRepository repository) =>
        {
            var conges = await repository.GetCongesByEmployeeIdAsync(employeeId);

            if (conges == null || !conges.Any())
                return Results.NotFound($"Aucun congé trouvé pour l'employé avec l'ID {employeeId}.");

            return Results.Ok(conges);
        });
    }
}
