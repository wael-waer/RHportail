
namespace PortailRH.API.Features.Conges.GetCongeById
{   
    public record GetAllCongesQuery() : IQuery<List<GetCongeByIdResult>>;
    public record GetCongeByIdResult(int Id, string TypeConge, DateTime DateDebut, DateTime DateFin, string Statut, string Motif, int EmployeeId);

    public class GetAllCongesQueryHandler(ICongeRepository congeRepository)
        : IQueryHandler<GetAllCongesQuery, List<GetCongeByIdResult>>
    {
        public async Task<List<GetCongeByIdResult>> Handle(GetAllCongesQuery query, CancellationToken cancellationToken)
        {
            var congés = await congeRepository.GetAllAsync();
            return congés.Select(c => new GetCongeByIdResult(
                c.Id,
                c.TypeConge,
                c.DateDebut,
                c.DateFin, 
                c.Statut,
                c.Motif,
                c.EmployeeId
               
            )).ToList();
        } 
    }
    


}