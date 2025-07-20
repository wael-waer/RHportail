namespace PortailRH.API.Features.Candidatures.GetAllCandidatures
{
    public record GetAllCandidaturesQuery() : IQuery<List<GetAllCandidaturesResult>>;

    public record GetAllCandidaturesResult(
        int Id,
        string Nom,
        string Prenom,
        string Email,
        string PosteSouhaite,
        string CVUrl
    );

    public class GetAllCandidaturesQueryHandler(ICandidatureRepository repository)
        : IQueryHandler<GetAllCandidaturesQuery, List<GetAllCandidaturesResult>>
    {
        public async Task<List<GetAllCandidaturesResult>> Handle(GetAllCandidaturesQuery query, CancellationToken cancellationToken)
        {
            var candidatures = await repository.GetAllAsync();
            return candidatures.Adapt<List<GetAllCandidaturesResult>>();
        }
    }
}
