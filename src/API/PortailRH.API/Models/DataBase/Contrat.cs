namespace PortailRH.API.Models.DataBase
{
    public class Contrat : EntityBase
    {
        public string Typecontrat { get; set; } 
        
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
       

        
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;
        
    }
    
}
    