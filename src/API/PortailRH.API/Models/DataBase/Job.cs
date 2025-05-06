namespace PortailRH.API.Models.DataBase
{
    public class Job : EntityBase
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string RequiredSkills { get; set; } = default!;
        public string Location { get; set; } = default!;
        public string ContractType { get; set; } = default!;
        public decimal Salary { get; set; }     
        public DateTime ApplicationDeadline { get; set; } 
        public DateTime PublicationDate { get; set; } 
        public string Status { get; set; } = default!;

        public List<Candidature> Applicants { get; set; } = new();
    }
}
