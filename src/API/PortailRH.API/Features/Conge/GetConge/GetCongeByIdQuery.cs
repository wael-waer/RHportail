
namespace PortailRH.API.Features.Conges.GetCongeById
{
    public record GetCongeByIdQuery(int Id) : IQuery<GetCongeByIdResult>;

    public record GetCongeByIdResult(int Id, string TypeConge, DateTime DateDebut, DateTime DateFin, string Statut, string Motif, int IdEmploye);

    public class GetCongeByIdQueryHandler(ICongeRepository congeRepository)
            : IQueryHandler<GetCongeByIdQuery, GetCongeByIdResult>
    {
        public async Task<GetCongeByIdResult> Handle(GetCongeByIdQuery query, CancellationToken cancellationToken)
        {
            var conge = await congeRepository.GetByIdAsync(query.Id);
            if (conge is null)
            {
                throw new NotFoundException("conge", query.Id);
            }
            var congeToReturn = conge.Adapt<GetCongeByIdResult>();

            return congeToReturn;
        }
    }
}