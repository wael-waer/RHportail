namespace PortailRH.API.Models.DataBase
{
    public class SuiviConge : EntityBase
    {
        public decimal SoldeInitial { get; set; }
        public decimal SoldeRestant { get; set; }
        public int Annee { get; set; }
        public bool Actif { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;
    }
}