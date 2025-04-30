namespace PortailRH.API.Features.Projects.UpdateProject
{
    public record UpdateProjectCommand(int Id, string Type, string Title, string Priority, DateTime StartDate, DateTime EndDate)
        : ICommand<UpdateProjectResult>;

    public record UpdateProjectResult(bool IsSuccess);

    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Priority).NotEmpty().WithMessage("Priority is required");
            RuleFor(x => x.StartDate).LessThan(x => x.EndDate).WithMessage("StartDate must be before EndDate");
        }
    }

    public class UpdateProjectCommandHandler(IProjectRepository projectRepository)
        : ICommandHandler<UpdateProjectCommand, UpdateProjectResult>
    {
        public async Task<UpdateProjectResult> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(command.Id);
            if (project is null)
            {
                throw new NotFoundException("Project", command.Id);
            }

            project.Type = command.Type;
            project.Title = command.Title;
            project.Priority = command.Priority;
            project.StartDate = command.StartDate;
            project.EndDate = command.EndDate;

            await projectRepository.UpdateAsync(project);

            return new UpdateProjectResult(true);
        }
    }
}
