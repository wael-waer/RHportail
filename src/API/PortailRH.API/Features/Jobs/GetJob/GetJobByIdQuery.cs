namespace PortailRH.API.Features.Jobs.GetAllJobs
{
    public record GetAllJobsQuery() : IQuery<List<GetAllJobsResult>>;

    public record GetAllJobsResult(int Id, string Title, string Description, string RequiredSkills, string Location,
        string ContractType, decimal Salary, DateTime ApplicationDeadline, DateTime PublicationDate, string Status);

    public class GetAllJobsQueryHandler(IJobRepository jobRepository)
        : IQueryHandler<GetAllJobsQuery, List<GetAllJobsResult>>
    {
        public async Task<List<GetAllJobsResult>> Handle(GetAllJobsQuery query, CancellationToken cancellationToken)
        {
            var jobs = await jobRepository.GetAllAsync();
            if (jobs is null || !jobs.Any())
            {
                throw new NotFoundException("Jobs");
            }

            var jobsToReturn = jobs.Adapt<List<GetAllJobsResult>>();
            return jobsToReturn;
        }
    }
}
