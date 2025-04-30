
namespace PortailRH.API.Features.Conges.UpdateConge
{
    public record UpdateCongeCommand(int Id, string TypeConge, DateTime DateDebut, DateTime DateFin, string Statut, string Motif, int IdEmploye)
        : ICommand<UpdateCongeResult>;

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

    public class UpdateCongeCommandHandler(ICongeRepository congeRepository, IEmployeeRepository employeeRepository)
        : ICommandHandler<UpdateCongeCommand, UpdateCongeResult>
    {
        public async Task<UpdateCongeResult> Handle(UpdateCongeCommand command, CancellationToken cancellationToken)
        {
            var employee = await employeeRepository.GetByIdAsync(command.IdEmploye);
            if (employee is null)
            {
                throw new NotFoundException("employee", command.IdEmploye);
            }
            var conge = await congeRepository.GetByIdAsync(command.Id);
            if (conge is null)
            {
                throw new NotFoundException("conge", command.Id);
            }

            conge.TypeConge = command.TypeConge;
            conge.DateDebut = command.DateDebut;
            conge.DateFin = command.DateFin;
            conge.Statut = command.Statut;
            conge.Motif = command.Motif;
            conge.IdEmploye = command.IdEmploye;

            await congeRepository.UpdateAsync(conge);
            return new UpdateCongeResult(true);
        }
    }
}
