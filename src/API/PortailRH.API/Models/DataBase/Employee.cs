
namespace PortailRH.API.Models.DataBase
{
    public class Employee: EntityBase
    {
      
        public string LastName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string Email { get; set; } = default!;

        public DateTime BirthDate { get; set; }
        public string Poste { get; set; } = default!;
        public decimal Salary { get; set; }
    }
}
