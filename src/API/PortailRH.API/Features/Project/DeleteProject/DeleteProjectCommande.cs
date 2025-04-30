namespace PortailRH.API.Features.Projects.DeleteProject
{
    public record DeleteProjectCommand(int Id) : ICommand<DeleteProjectResult>;
    public record DeleteProjectResult(bool IsSuccess);
    public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than zero");
        }
    }
    public class DeleteProjectCommandHandler(IProjectRepository repository)
       : ICommandHandler<DeleteProjectCommand, DeleteProjectResult>
    {
        public async Task<DeleteProjectResult> Handle(DeleteProjectCommand command, CancellationToken cancellationToken)
        {
            var existing = await repository.GetByIdAsync(command.Id);
            if (existing is null)
                return new DeleteProjectResult(false);

            await repository.DeleteAsync(existing);
            return new DeleteProjectResult(true);
        }
    }
}
