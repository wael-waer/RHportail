namespace PortailRH.API.Features.Projects.GetAllProjects
{
    public record GetAllProjectsQuery() : IQuery<List<GetAllProjectsResult>>;

    public record GetAllProjectsResult(
        int Id,
        string Type,
        string Title,
        string Priority,
        DateTime StartDate,
        DateTime EndDate
    );

    public class GetAllProjectsQueryHandler(IProjectRepository projectRepository)
        : IQueryHandler<GetAllProjectsQuery, List<GetAllProjectsResult>>
    {
        public async Task<List<GetAllProjectsResult>> Handle(GetAllProjectsQuery query, CancellationToken cancellationToken)
        {
            var projects = await projectRepository.GetAllAsync();
            return projects.Adapt<List<GetAllProjectsResult>>();
        }
    }
}
