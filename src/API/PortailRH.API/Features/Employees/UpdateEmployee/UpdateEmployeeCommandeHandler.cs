namespace PortailRH.API.Features.Employees.UpdateEmployee
{
    public record UpdateEmployeeCommand(
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
    ) : ICommand<UpdateEmployeeResult>;

    public record UpdateEmployeeResult(bool IsSuccess);

    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("L'ID est requis.");
            RuleFor(x => x.Nom).NotEmpty().WithMessage("Le nom est obligatoire.");
            RuleFor(x => x.Prenom).NotEmpty().WithMessage("Le prénom est obligatoire.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email invalide.");
            RuleFor(x => x.DateNaissance).LessThan(DateTime.Today).WithMessage("La date de naissance doit être dans le passé.");
            RuleFor(x => x.NumeroTelephone).NotEmpty().WithMessage("Le numéro de téléphone est obligatoire.");
            RuleFor(x => x.NumeroIdentification).NotEmpty().WithMessage("Le numéro d'identification est obligatoire.");
            RuleFor(x => x.SoldeConge).GreaterThanOrEqualTo(0).WithMessage("Le solde de congé doit être positif ou nul.");
            RuleFor(x => x.TypeContrat).NotEmpty().WithMessage("Le type de contrat est obligatoire.");
            RuleFor(x => x.Salaire).GreaterThanOrEqualTo(0).WithMessage("Le salaire doit être supérieur ou égal à zéro.");
        }
    }

    public class UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        : ICommandHandler<UpdateEmployeeCommand, UpdateEmployeeResult>
    {
        public async Task<UpdateEmployeeResult> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = await employeeRepository.GetByIdAsync(command.Id);
            if (employee is null)
                throw new NotFoundException("Employé", command.Id);

            employee.Nom = command.Nom;
            employee.Prenom = command.Prenom;
            employee.Email = command.Email;
            employee.DateNaissance = command.DateNaissance;
            employee.NumeroTelephone = command.NumeroTelephone;
            employee.EmailSecondaire = command.EmailSecondaire;
            employee.NumeroTelephoneSecondaire = command.NumeroTelephoneSecondaire;
            employee.NumeroIdentification = command.NumeroIdentification;
            employee.SoldeConge = command.SoldeConge;
            employee.TypeContrat = command.TypeContrat;
            employee.Salaire = command.Salaire;

            await employeeRepository.UpdateAsync(employee);
            return new UpdateEmployeeResult(true);
        }
    }
}
