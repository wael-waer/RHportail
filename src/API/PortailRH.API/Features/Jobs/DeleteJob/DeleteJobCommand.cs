namespace PortailRH.API.Features.Jobs.DeleteJob
{
    public record DeleteJobCommand(int Id) : ICommand<DeleteJobResult>;

    public record DeleteJobResult(bool IsSuccess);

    public class DeleteJobCommandValidator : AbstractValidator<DeleteJobCommand>
    {
        public DeleteJobCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than zero");
        }
    }
    public class DeleteJobCommandHandler(IJobRepository jobRepository)
        : ICommandHandler<DeleteJobCommand, DeleteJobResult>
    {
        public async Task<DeleteJobResult> Handle(DeleteJobCommand command, CancellationToken cancellationToken)
        {
            var job = await jobRepository.GetByIdAsync(command.Id);
            if (job is null)
            {
                throw new NotFoundException("Job", command.Id);
            }

            await jobRepository.DeleteAsync(job);
            return new DeleteJobResult(true);
        }
    }
}
