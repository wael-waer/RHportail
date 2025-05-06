namespace PortailRH.API.Models.DataBase
{


    public class Candidature : EntityBase
    {

        public string Nom { get; set; } = default!;
        public string Prenom { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string PosteSouhaite { get; set; }

    }
}

