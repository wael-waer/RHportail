namespace PortailRH.API.Features.Projects.GetProjectById
{
    public record GetProjectByIdQuery(int Id) : IQuery<GetProjectByIdResult>;

    public record GetProjectByIdResult(int Id,string Type,string Title,string Priority,DateTime StartDate, DateTime EndDate);
    public class GetProjectByIdQueryHandler(IProjectRepository projectRepository)
       : IQueryHandler<GetProjectByIdQuery, GetProjectByIdResult>
    {
        public async Task<GetProjectByIdResult> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetByIdAsync(query.Id);
            if (project is null)
                throw new NotFoundException("project", query.Id);

            return project.Adapt<GetProjectByIdResult>();
        }
    }

}
