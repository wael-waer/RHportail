namespace PortailRH.API.Features.Employees.UpdateEmployee
{
    public record UpdateEmployeeRequest(
       int Id,
        string Nom,
        string Prenom,
        string Email,
        DateTime DateNaissance,
        string Fonction,
        string Etablissement,
        DateTime DateEntree,
        string NumeroTelephone,
        string? EmailSecondaire,
        string? NumeroTelephoneSecondaire,
        string NumeroIdentification,
        int SoldeConge,
        string TypeContrat,
        decimal Salaire
    );

    public record UpdateEmployeeResponse(bool IsSuccess);

    public class UpdateEmployeeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/employees/{id:int}", async (int id, UpdateEmployeeRequest request, ISender sender) =>
            {
                var command = new UpdateEmployeeCommand(
                    Id: id,
                    Nom: request.Nom,
                    Prenom: request.Prenom,
                    Email: request.Email,
                    DateNaissance: request.DateNaissance,
                    Fonction: request.Fonction,
                    Etablissement: request.Etablissement,
                    DateEntree: request.DateEntree,
                    NumeroTelephone: request.NumeroTelephone,
                    EmailSecondaire: request.EmailSecondaire,
                    NumeroTelephoneSecondaire: request.NumeroTelephoneSecondaire,
                    NumeroIdentification: request.NumeroIdentification,
                    SoldeConge: request.SoldeConge,
                    TypeContrat: request.TypeContrat,
                    Salaire: request.Salaire
                );

                var result = await sender.Send(command);
                var response = new UpdateEmployeeResponse(result.IsSuccess);
                return Results.Ok(response);
            })
            .WithName("UpdateEmployee")
            .Produces<UpdateEmployeeResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Employee")
            .WithDescription("Update an existing employee with full data.");
        }
    }
}
