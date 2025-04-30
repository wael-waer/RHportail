
namespace PortailRH.API.Features.Conges.DeleteConge
{
    public record DeleteCongeCommand(int Id) : ICommand<DeleteCongeResult>;

    public record DeleteCongeResult(bool IsSuccess);

    public class DeleteCongeCommandValidator : AbstractValidator<DeleteCongeCommand>
    {
        public DeleteCongeCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than zero");
        }
    }
    public class DeleteCongeCommandHandler(ICongeRepository congeRepository)
        : ICommandHandler<DeleteCongeCommand, DeleteCongeResult>
    {
        public async Task<DeleteCongeResult> Handle(DeleteCongeCommand command, CancellationToken cancellationToken)
        {
            var conge = await congeRepository.GetByIdAsync(command.Id);

            if (conge == null)
            {
                return new DeleteCongeResult(false);
            }

            await congeRepository.DeleteAsync(conge);

            return new DeleteCongeResult(true);
        }
    }
}
