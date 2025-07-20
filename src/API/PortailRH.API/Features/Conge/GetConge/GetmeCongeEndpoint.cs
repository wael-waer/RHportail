


namespace PortailRH.API.Features.Conges.GetMeConge
{
    public class GetMeCongesEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/employes/me/conges", async (
                HttpContext httpContext,
                IEmployeeRepository employeeRepo,
                ICongeRepository congeRepo
            ) =>
            {
                var userEmail = httpContext.User.Identity?.Name;

                if (string.IsNullOrEmpty(userEmail))
                    return Results.Unauthorized();

                var employee = await employeeRepo.GetByEmailAsync(userEmail);
                if (employee is null)
                    return Results.NotFound("Employé non trouvé");

                var conges = await congeRepo.GetCongesByEmployeeIdAsync(employee.Id);

                return Results.Ok(conges);
            })
            .RequireAuthorization()
            .WithName("GetMyConges")
            .Produces<IReadOnlyList<Conge>>(StatusCodes.Status200OK)
            .WithSummary("Récupérer les congés de l'utilisateur connecté")
            .WithDescription("Retourne la liste des congés créés par l'employé connecté.");
        }
    }
}
