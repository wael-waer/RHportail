namespace PortailRH.API.Features.Candidatures.GetCandidatureById
{

    public record GetCandidatureByIdQuery(int Id) : IQuery<GetCandidatureByIdResult>;

    public record GetCandidatureByIdResult(int Id, string Nom, string Prenom, string Email, string PosteSouhaite);

    public class GetCandidatureByIdQueryHandler(ICandidatureRepository repository)
        : IQueryHandler<GetCandidatureByIdQuery, GetCandidatureByIdResult>
    {
        public async Task<GetCandidatureByIdResult> Handle(GetCandidatureByIdQuery query, CancellationToken cancellationToken)
        {
            var candidature = await repository.GetByIdAsync(query.Id);
            if (candidature is null)
            {
                throw new NotFoundException("candidature", query.Id);
            }

            return candidature.Adapt<GetCandidatureByIdResult>();
        }
    }
}
