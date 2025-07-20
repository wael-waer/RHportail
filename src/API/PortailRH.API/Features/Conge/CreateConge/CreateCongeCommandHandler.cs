namespace PortailRH.API.Features.Conges.CreateConge
{
    public record CreateCongeCommand(
         string TypeConge,
         DateTime DateDebut,
         DateTime DateFin,
         string Statut,
         string Motif,
         int EmployeeId 
     ) : ICommand<CreateCongeResult>;

    public record CreateCongeResult(int Id);

    public class CreateCongeCommandValidator : AbstractValidator<CreateCongeCommand>
    {
        public CreateCongeCommandValidator()
        {
            RuleFor(x => x.TypeConge).NotEmpty().WithMessage("TypeConge is required");
            RuleFor(x => x.DateDebut).LessThan(x => x.DateFin).WithMessage("DateDebut must be before DateFin");
            RuleFor(x => x.Statut).NotEmpty().WithMessage("Statut is required");
            RuleFor(x => x.Motif).NotEmpty().WithMessage("Motif is required");
            RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("EmployeeId is required");
        }
    }
    public class CreateCongeCommandHandler(
        IEmployeeRepository employeeRepository,
        ICongeRepository congeRepository
    ) : ICommandHandler<CreateCongeCommand, CreateCongeResult>
    {
        public async Task<CreateCongeResult> Handle(CreateCongeCommand command, CancellationToken cancellationToken)
        {
            var employee = await employeeRepository.GetByIdAsync(command.EmployeeId);
            if (employee is null)
                throw new Exception("Employé introuvable");

            var joursDemandes = (command.DateFin - command.DateDebut).TotalDays;
            var anneeConge = command.DateDebut.Year;


            var suiviConge = await employeeRepository.GetSuiviCongeAsync(command.EmployeeId, anneeConge);
            if (suiviConge == null || !suiviConge.Actif)
                throw new Exception("Aucun suivi de congé trouvé pour l'année en cours.");

            if (command.Statut == "Approuve")
            {
                // ⚠️ Vérifie le solde
                if (suiviConge.SoldeRestant < (decimal)joursDemandes)
                    throw new Exception("Solde de congé insuffisant.");

                // ➖ Met à jour le solde restant
                suiviConge.SoldeRestant -= (decimal)joursDemandes;
                await employeeRepository.UpdateSuiviCongeAsync(suiviConge);
            }

            var conge = new Conge
            {
                TypeConge = command.TypeConge,
                DateDebut = command.DateDebut,
                DateFin = command.DateFin,
                Statut = command.Statut,
                Motif = command.Motif,
                
                EmployeeId = command.EmployeeId
            };

            await congeRepository.AddAsync(conge);
            return new CreateCongeResult(conge.Id);
        }
    }
}