
namespace PortailRH.API.Models.DataBase
{
    public class Employee: EntityBase
    {

        public string Prenom { get; set; } = default!;

        
        public string Nom { get; set; } = default!;

        
        public string Email { get; set; } = default!;

        
        public string MotDePasse { get; set; } = default!;

        
        public DateTime DateNaissance { get; set; }

       
        public string Fonction { get; set; } = default!;

        
        public string Etablissement { get; set; } = default!;

        public DateTime DateEntree { get; set; }

       
        public string NumeroTelephone { get; set; } = default!;

       
        public string? EmailSecondaire { get; set; }

        public string? NumeroTelephoneSecondaire { get; set; }

        
        public string NumeroIdentification { get; set; } = default!;

        
        public int SoldeConge { get; set; }

        
        public int SoldeCongeMaladie { get; set; }

       
        public decimal Salaire { get; set; }
        public ICollection<Conge> Conges { get; set; } = new List<Conge>();
        public ICollection<SuiviConge> SuiviConges { get; set; } = new List<SuiviConge>();


    }
}
