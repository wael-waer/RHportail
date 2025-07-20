namespace PortailRH.API.Features.Employees.GetAllEmployees
{
    public record GetAllEmployeesQuery : IQuery<List<GetAllEmployeesResult>>;

    public record GetAllEmployeesResult(
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

    public class GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository)
        : IQueryHandler<GetAllEmployeesQuery, List<GetAllEmployeesResult>>
    {
        public async Task<List<GetAllEmployeesResult>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
        {
            var employees = await employeeRepository.GetAllAsync();

            return employees.Select(e => new GetAllEmployeesResult(
                e.Id,
                e.Nom,
                e.Prenom,
                e.Email,
                e.DateNaissance,
                e.Fonction,
                e.Etablissement,
                e.DateEntree,
                e.NumeroTelephone,
                e.EmailSecondaire,
                e.NumeroTelephoneSecondaire,
                e.NumeroIdentification,
                e.SoldeConge,
                e.SoldeCongeMaladie,
                e.Salaire
            )).ToList();
        }
    }
}
