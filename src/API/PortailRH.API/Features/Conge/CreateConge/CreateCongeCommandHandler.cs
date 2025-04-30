namespace PortailRH.API.Features.Conges.CreateConge
{
    public record CreateCongeCommand(string TypeConge, DateTime DateDebut, DateTime DateFin, string Statut, string Motif, int IdEmploye)
        : ICommand<CreateCongeResult>;
    public record CreateCongeResult(int Id);

public class CreateCongeCommandValidator : AbstractValidator<CreateCongeCommand>
{
    public CreateCongeCommandValidator()
    {
        RuleFor(x => x.TypeConge).NotEmpty().WithMessage("TypeConge is required");
        RuleFor(x => x.DateDebut).LessThan(x => x.DateFin).WithMessage("DateDebut must be before DateFin");
        RuleFor(x => x.Statut).NotEmpty().WithMessage("Statut is required");
        RuleFor(x => x.Motif).NotEmpty().WithMessage("Motif is required");
        RuleFor(x => x.IdEmploye).GreaterThan(0).WithMessage("IdEmploye is required");
    }
}
   public class CreateCongeCommandHandler(ICongeRepository congeRepository)
        : ICommandHandler<CreateCongeCommand, CreateCongeResult>
   {
      public async Task<CreateCongeResult> Handle(CreateCongeCommand command, CancellationToken cancellationToken)
      {

        var congeToCreate = command.Adapt<Conge>();
        var conge = await congeRepository.AddAsync(congeToCreate);
        return new CreateCongeResult(conge.Id);
      }
   }
}