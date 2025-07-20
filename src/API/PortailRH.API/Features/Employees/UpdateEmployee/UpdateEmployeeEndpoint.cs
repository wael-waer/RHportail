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
        int SoldeCongeMaladie,
        decimal Salaire
    );

    public record UpdateEmployeeResponse(bool IsSuccess);

    public class UpdateEmployeeEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/employees/{id:int}", async (int id, UpdateEmployeeRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateEmployeeCommand>() with { Id = id };
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateEmployeeResponse>();
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
