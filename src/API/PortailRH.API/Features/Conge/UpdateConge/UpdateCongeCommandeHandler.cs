namespace PortailRH.API.Features.Conges.UpdateConge
{
    public record UpdateCongeCommand(
        int Id,
        string TypeConge,
        DateTime DateDebut,
        DateTime DateFin,
        string Statut,
        string Motif,
        int EmployeeId
    ) : ICommand<UpdateCongeResult>;

    public record UpdateCongeResult(bool IsSuccess);

    public class UpdateCongeCommandValidator : AbstractValidator<UpdateCongeCommand>
    {
        public UpdateCongeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.TypeConge).NotEmpty().WithMessage("TypeConge is required");
            RuleFor(x => x.DateDebut).LessThan(x => x.DateFin).WithMessage("DateDebut must be earlier than DateFin");
            RuleFor(x => x.DateFin).GreaterThan(x => x.DateDebut).WithMessage("DateFin must be later than DateDebut");
            RuleFor(x => x.Statut).NotEmpty().WithMessage("Statut is required");
            RuleFor(x => x.Motif).NotEmpty().WithMessage("Motif is required");
        }
    }

    public class UpdateCongeCommandHandler(
        ICongeRepository congeRepository,
        IEmployeeRepository employeeRepository
    ) : ICommandHandler<UpdateCongeCommand, UpdateCongeResult>
    {
        public async Task<UpdateCongeResult> Handle(UpdateCongeCommand command, CancellationToken cancellationToken)
        {
            var employee = await employeeRepository.GetByIdAsync(command.EmployeeId);
            if (employee is null)
            {
                throw new NotFoundException("employee", command.EmployeeId);
            }

            var conge = await congeRepository.GetByIdAsync(command.Id);
            if (conge is null)
            {
                throw new NotFoundException("conge", command.Id);
            }

            var ancienStatut = conge.Statut;
            var ancienJours = (conge.DateFin - conge.DateDebut).TotalDays ;
            var nouveauxJours = (command.DateFin - command.DateDebut).TotalDays ;
            var anneeConge = command.DateDebut.Year;

            if (ancienStatut != "Approved" && command.Statut == "Approved")
            {
                var suiviConge = await employeeRepository.GetSuiviCongeAsync(command.EmployeeId,anneeConge);
                if (suiviConge == null || !suiviConge.Actif)
                    throw new Exception("Aucun suivi de congé trouvé pour l'année en cours.");

                if (suiviConge.SoldeRestant < (decimal)nouveauxJours)
                    throw new Exception("Solde de congé insuffisant.");

                suiviConge.SoldeRestant -= (decimal)nouveauxJours;
                await employeeRepository.UpdateSuiviCongeAsync(suiviConge);
            }

            // Optional: gestion du cas où le congé était déjà approuvé et on modifie les dates
            else if (ancienStatut == "Approved" && command.Statut == "Approved" &&
                     (command.DateDebut != conge.DateDebut || command.DateFin != conge.DateFin))
            {
                var suiviConge = await employeeRepository.GetSuiviCongeAsync(command.EmployeeId,anneeConge);
                if (suiviConge == null)
                    throw new Exception("Aucun suivi de congé trouvé pour l'année en cours.");

                var difference = (decimal)(nouveauxJours - ancienJours);

                if (difference > 0 && suiviConge.SoldeRestant < difference)
                    throw new Exception("Solde de congé insuffisant pour la nouvelle durée.");

                suiviConge.SoldeRestant -= difference;
                await employeeRepository.UpdateSuiviCongeAsync(suiviConge);
            }

            conge.TypeConge = command.TypeConge;
            conge.DateDebut = command.DateDebut;
            conge.DateFin = command.DateFin;
            conge.Statut = command.Statut;
            conge.Motif = command.Motif;
            conge.EmployeeId = command.EmployeeId;

            await congeRepository.UpdateAsync(conge);

            return new UpdateCongeResult(true);
        }
    }
}
