namespace PortailRH.API.Features.Conges.GetCongeStatusById
{
    public record GetCongeStatusByIdQuery(int CongeId) : IQuery<GetCongeStatusByIdResult>;

    public record GetCongeStatusByIdResult(int Id, string Statut);
    public class GetCongeStatusByIdHandler(ICongeRepository congeRepository)
        : IQueryHandler<GetCongeStatusByIdQuery, GetCongeStatusByIdResult>
    {
        public async Task<GetCongeStatusByIdResult> Handle(GetCongeStatusByIdQuery query, CancellationToken cancellationToken)
        {
            var conge = await congeRepository.GetByIdAsync(query.CongeId);

            if (conge is null)
                throw new NotFoundException($"Aucun congé trouvé avec l'ID {query.CongeId}");

            return new GetCongeStatusByIdResult(conge.Id, conge.Statut);
        }
    }
}
