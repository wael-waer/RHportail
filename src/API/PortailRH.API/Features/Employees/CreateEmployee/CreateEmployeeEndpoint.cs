

namespace PortailRH.API.Features.Employees.CreateEmployee
{
    public record CreateEmployeeRequest(
         string Nom,
         string Prenom,
         string Email,
         string MotDePasse,
         DateTime DateNaissance,
         string Fonction,
         string Etablissement,
         DateTime DateEntree,
         string NumeroTelephone,
         string? EmailSecondaire,
         string? NumeroTelephoneSecondaire,
         string NumeroIdentification,
         int SoldeConge,
         int SoldeCongeMaladie,
         decimal Salaire
     );

    public record CreateEmployeeResponse(int Id);

    public class CreateEmployeeEndpoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/employes", async (CreateEmployeeRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateEmployeeCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateEmployeeResponse>();
                return Results.Created($"/api/employes/{response.Id}", response);
            })
            .WithName("CreateEmploye")
            .Produces<CreateEmployeeResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Créer un employé")
            .WithDescription("Crée un nouvel employé avec toutes les informations nécessaires.");
        }
    }
}
