namespace PortailRH.API.Features.Jobs.UpdateJob
{
    public record UpdateJobCommand(int Id, string Title, string Description, string RequiredSkills, string Location,
        string ContractType, decimal Salary, DateTime ApplicationDeadline, DateTime PublicationDate, string Status)
        : ICommand<UpdateJobResult>;

    public record UpdateJobResult(bool IsSuccess);
    public class UpdateJobCommandValidator : AbstractValidator<UpdateJobCommand>
    {
        public UpdateJobCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.RequiredSkills).NotEmpty();
            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.ContractType).NotEmpty();
            RuleFor(x => x.Salary).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Status).NotEmpty();
        }
    }
    public class UpdateJobCommandHandler(IJobRepository repository)
        : ICommandHandler<UpdateJobCommand, UpdateJobResult>
    {
        public async Task<UpdateJobResult> Handle(UpdateJobCommand command, CancellationToken cancellationToken)
        {
            var job = await repository.GetByIdAsync(command.Id);
            if (job is null)
            {
                throw new NotFoundException("job", command.Id);
            }

            job.Title = command.Title;
            job.Description = command.Description;
            job.RequiredSkills = command.RequiredSkills;
            job.Location = command.Location;
            job.ContractType = command.ContractType;
            job.Salary = command.Salary;
            job.ApplicationDeadline = command.ApplicationDeadline;
            job.PublicationDate = command.PublicationDate;
            job.Status = command.Status;

            await repository.UpdateAsync(job);
            return new UpdateJobResult(true);
        }
    }

}