namespace PortailRH.API.Features.Jobs.Createjob
{
    public record CreateJobCommand(string Title,
        string Description,string RequiredSkills,string Location,string ContractType,decimal Salary,DateTime ApplicationDeadline,DateTime PublicationDate,string Status) 
        : ICommand<CreateJobResult>;

    public record CreateJobResult(int Id);

    public class CreateJobValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.RequiredSkills).NotEmpty();
            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.ContractType).NotEmpty();
            RuleFor(x => x.Salary).GreaterThan(0);
            RuleFor(x => x.ApplicationDeadline).GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(x => x.PublicationDate).LessThanOrEqualTo(DateTime.Today);
            RuleFor(x => x.Status).NotEmpty();
        }
    }

    public class CreateJobCommandHandler(IJobRepository jobRepository)
        : ICommandHandler<CreateJobCommand, CreateJobResult>
    {
        public async Task<CreateJobResult> Handle(CreateJobCommand command, CancellationToken cancellationToken)
        {
            var jobToCreate = command.Adapt<Job>();
            var job = await jobRepository.AddAsync(jobToCreate);
            return new CreateJobResult(job.Id);
        }
    }
}
