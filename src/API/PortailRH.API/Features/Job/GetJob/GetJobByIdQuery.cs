namespace PortailRH.API.Features.Jobs.GetJobById
{
    public record GetJobByIdQuery(int Id) : IQuery<GetJobByIdResult>;

    public record GetJobByIdResult(int Id, string Title, string Description, string RequiredSkills, string Location,
        string ContractType, decimal Salary, DateTime ApplicationDeadline, DateTime PublicationDate, string Status);

    public class GetJobByIdQueryHandler(IJobRepository jobRepository)
        : IQueryHandler<GetJobByIdQuery, GetJobByIdResult>
    {
        public async Task<GetJobByIdResult> Handle(GetJobByIdQuery query, CancellationToken cancellationToken)
        {
            var job = await jobRepository.GetByIdAsync(query.Id);
            if (job is null)
            {
                throw new NotFoundException("Job", query.Id);
            }

            var jobToReturn = job.Adapt<GetJobByIdResult>();
            return jobToReturn;
        }
    }
}
