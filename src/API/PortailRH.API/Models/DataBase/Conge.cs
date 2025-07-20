
namespace PortailRH.API.Models.DataBase
{
    public class Conge : EntityBase
    {
        public string TypeConge { get; set; } = default!;
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string Statut { get; set; } = default!;
        public string Motif { get; set; } = default!;
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;
    }
}
