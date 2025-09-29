namespace PortailRH.API.Models.DataBase
{
    public class Equipe : EntityBase
    {
        public Guid Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DateCreation { get; set; }
        public int ResponsableId { get; set; }
       

        // Navigation properties
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }

   
}