
namespace PortailRH.API.Features.Projects.CreateProject
{

    public record CreateProjectCommand(string Type, string Title, string Priority, DateTime StartDate, DateTime EndDate)
        : ICommand<CreateProjectResult>;

    public record CreateProjectResult(int Id);

    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Type).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Priority).NotEmpty();
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate);
            RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        }
    }

    public class CreateProjectCommandHandler(IProjectRepository projectRepository)
        : ICommandHandler<CreateProjectCommand, CreateProjectResult>
    {
        public async Task<CreateProjectResult> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
        {
            var projectToCreate = command.Adapt<Project>();
            var project = await projectRepository.AddAsync(projectToCreate);
            return new CreateProjectResult(project.Id);
        }
    }
}
