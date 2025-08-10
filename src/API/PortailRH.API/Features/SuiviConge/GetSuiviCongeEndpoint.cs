using PortailRH.API.Contracts;
using PortailRH.API.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PortailRH.API.Features.SuiviConges
{
    public class SuiviCongeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/employees/{employeeId}/suiviconges", async (
                int employeeId,
                IEmployeeRepository employeeRepository, ISuiviCongeRepository suiviCongeRepository) =>
            {
                var suivis = await suiviCongeRepository.GetAllSuivisCongeByEmployeeIdAsync(employeeId);

                if (suivis == null || suivis.Count == 0)
                    return Results.NotFound("Aucun suivi de congé trouvé pour cet employé.");

                // Projection : ne retourner que les champs souhaités
                var result = suivis.Select(s => new
                {
                    s.EmployeeId,
                    s.Annee,
                    s.SoldeInitial,
                    s.SoldeRestant,
                    s.Actif
                });

                return Results.Ok(result);
            });
            app.MapGet("/api/employees/{employeeId}/suiviconges/archives", async (
     int employeeId,
     ISuiviCongeRepository suiviCongeRepository,
     ICongeRepository congeRepository) =>
            {
                var currentYear = DateTime.UtcNow.Year;

                // Récupère tous les suivis
                var allSuivis = await suiviCongeRepository.GetAllSuivisCongeByEmployeeIdAsync(employeeId);

                // Filtrer uniquement les années archivées réellement présentes
                var archivedSuivis = allSuivis
                    .Where(s => !s.Actif)
                    .Select(s => new
                    {
                        s.EmployeeId,
                        s.Annee,
                        s.SoldeInitial,
                        s.SoldeRestant,
                        s.Actif
                    })
                    .ToList();  

                return Results.Ok(archivedSuivis);

            });



        }
    }
}
