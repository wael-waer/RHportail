namespace PortailRH.API.Models.DataBase
{
    public class Admin : EntityBase
    {
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
    }
}
