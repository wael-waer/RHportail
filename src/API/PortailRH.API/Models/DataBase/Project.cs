namespace PortailRH.API.Models.DataBase
{

public class Project : EntityBase
{
    public string Type { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Priority { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
}
