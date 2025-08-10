namespace PortailRH.API.Features.Employees.GetAllEmployees
{
    // Requête pour récupérer tous les employés
    public record GetAllEmployeesQuery : IQuery<List<GetEmployeeResult>>;

    // Requête pour récupérer un seul employé par ID
    public record GetEmployeeByIdQuery(int Id) : IQuery<GetEmployeeResult?>;

    // Résultat commun pour les deux requêtes
    public record GetEmployeeResult(
        int Id,
        string Nom,
        string Prenom,
        string Email,
        DateTime DateNaissance,
        string Fonction,
        string MotDePasse,
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

    // Handler pour récupérer tous les employés
    public class GetAllEmployeesQueryHandler(IEmployeeRepository employeeRepository)
        : IQueryHandler<GetAllEmployeesQuery, List<GetEmployeeResult>>
    {
        public async Task<List<GetEmployeeResult>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
        {
            var employees = await employeeRepository.GetAllAsync();

            return employees.Select(e => new GetEmployeeResult(
                e.Id,
                e.Nom,
                e.Prenom,
                e.Email,
                e.DateNaissance,
                e.Fonction,
                e.MotDePasse,
                e.Etablissement,
                e.DateEntree,
                e.NumeroTelephone,
                e.EmailSecondaire,
                e.NumeroTelephoneSecondaire,
                e.NumeroIdentification,
                e.SoldeConge,
                e.TypeContrat,
                e.Salaire
            )).ToList();
        }
    }

    // Handler pour récupérer un employé par ID
    public class GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository)
        : IQueryHandler<GetEmployeeByIdQuery, GetEmployeeResult?>
    {
        public async Task<GetEmployeeResult?> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {
            var employee = await employeeRepository.GetByIdAsync(query.Id);

            if (employee == null)
                return null;

            return new GetEmployeeResult(
                employee.Id,
                employee.Nom,
                employee.Prenom,
                employee.Email,
                employee.DateNaissance,
                employee.Fonction,
                employee.MotDePasse,
                employee.Etablissement,
                employee.DateEntree,
                employee.NumeroTelephone,
                employee.EmailSecondaire,
                employee.NumeroTelephoneSecondaire,
                employee.NumeroIdentification,
                employee.SoldeConge,
                employee.TypeContrat,
                employee.Salaire
            );
        }
    }
}
