namespace PortailRH.API.Features.Equipes.CreateEquipe
{
    public record AssignerEmployesCommand(Guid EquipeId, List<int> EmployeeIds) : IRequest<AssignerEmployesResult>;

    public record AssignerEmployesResult(bool Success, string Message);

    public class AssignerEmployesCommandValidator : AbstractValidator<AssignerEmployesCommand>
    {
        public AssignerEmployesCommandValidator()
        {
            RuleFor(x => x.EquipeId)
                .NotEmpty().WithMessage("L'ID de l'équipe est requis");

            RuleFor(x => x.EmployeeIds)
                .NotEmpty().WithMessage("Au moins un employé doit être sélectionné")
                .Must(ids => ids != null && ids.Any()).WithMessage("La liste des employés ne peut pas être vide")
                .Must(HaveUniqueIds).WithMessage("Les IDs des employés doivent être uniques");

            RuleForEach(x => x.EmployeeIds)
                .GreaterThan(0).WithMessage("L'ID de l'employé doit être positif");
        }

        private bool HaveUniqueIds(List<int> employeeIds)
            => employeeIds.Distinct().Count() == employeeIds.Count;
    }

    public class AssignerEmployesCommandHandler : IRequestHandler<AssignerEmployesCommand, AssignerEmployesResult>
    {
        private readonly IEquipeRepository _equipeRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AssignerEmployesCommandHandler(
            IEquipeRepository equipeRepository,
            IEmployeeRepository employeeRepository)
        {
            _equipeRepository = equipeRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<AssignerEmployesResult> Handle(AssignerEmployesCommand command, CancellationToken cancellationToken)
        {
            var equipe = await _equipeRepository.GetByIdAsync(command.EquipeId);
            if (equipe == null)
                return new AssignerEmployesResult(false, "L'équipe n'existe pas");

            var employeesExist = await ValidateEmployeesExist(command.EmployeeIds);
            if (!employeesExist)
                return new AssignerEmployesResult(false, "Un ou plusieurs employés n'existent pas");

            await _equipeRepository.AssignEmployeesToEquipeAsync(
                command.EquipeId,
                command.EmployeeIds
            );

            return new AssignerEmployesResult(true, "Employés assignés avec succès");
        }

        private async Task<bool> ValidateEmployeesExist(IEnumerable<int> employeeIds)
        {
            foreach (var employeeId in employeeIds)
            {
                if (await _employeeRepository.GetByIdAsync(employeeId) == null) return false;
            }
            return true;
        }
    }
}
