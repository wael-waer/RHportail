namespace PortailRH.API.Features.Equipes.CreateEquipe
{
    public record CreerEquipeCommand(
        string Nom,
        string? Description,
        List<int> EmployeeIds,
        int ResponsableId
    ) : IRequest<CreerEquipeResult>;

    public record CreerEquipeResult(bool Success, string Message, Guid? EquipeId = null);

    public class CreerEquipeCommandValidator : AbstractValidator<CreerEquipeCommand>
    {
        public CreerEquipeCommandValidator()
        {
            RuleFor(x => x.Nom)
                .NotEmpty().WithMessage("Le nom de l'équipe est requis")
                .MaximumLength(100).WithMessage("Le nom ne peut pas dépasser 100 caractères");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La description ne peut pas dépasser 500 caractères");

            RuleFor(x => x.EmployeeIds)
                .NotEmpty().WithMessage("Au moins un employé doit être sélectionné")
                .Must(ids => ids != null && ids.Any()).WithMessage("La liste des employés ne peut pas être vide")
                .Must(HaveUniqueIds).WithMessage("Les IDs des employés doivent être uniques");

            RuleFor(x => x.ResponsableId)
                .GreaterThan(0).WithMessage("L'ID du responsable est requis");

            RuleForEach(x => x.EmployeeIds)
                .GreaterThan(0).WithMessage("L'ID de l'employé doit être positif");
        }

        private bool HaveUniqueIds(List<int> employeeIds)
            => employeeIds.Distinct().Count() == employeeIds.Count;
    }

    public class CreerEquipeCommandHandler : IRequestHandler<CreerEquipeCommand, CreerEquipeResult>
    {
        private readonly IEquipeRepository _equipeRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public CreerEquipeCommandHandler(
            IEquipeRepository equipeRepository,
            IEmployeeRepository employeeRepository)
        {
            _equipeRepository = equipeRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<CreerEquipeResult> Handle(CreerEquipeCommand command, CancellationToken cancellationToken)
        {
            var employeesExist = await ValidateEmployeesExist(command.EmployeeIds);
            if (!employeesExist)
                return new CreerEquipeResult(false, "Un ou plusieurs employés n'existent pas");

            var equipe = new Equipe
            {
                Id = Guid.NewGuid(),
                Nom = command.Nom,
                Description = command.Description,
                ResponsableId = command.ResponsableId,
                DateCreation = DateTime.UtcNow
            };

            var equipeCreee = await _equipeRepository.CreateEquipeWithEmployeesAsync(
                equipe,
                command.EmployeeIds
            );

            return new CreerEquipeResult(true, "Équipe créée avec succès", equipeCreee.Id);
        }

        private async Task<bool> ValidateEmployeesExist(IEnumerable<int> employeeIds)
        {
            foreach (var employeeId in employeeIds)
            {
                var employee = await _employeeRepository.GetByIdAsync(employeeId);
                if (employee == null) return false;
            }
            return true;
        }
    }
}
