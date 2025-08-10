
namespace PortailRH.API.Models.DataBase
{
    public class PortailRHContext:DbContext
    {
        public PortailRHContext(DbContextOptions<PortailRHContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; } = default!;
        public DbSet<Conge> Conges { get; set; } = default!;
        public DbSet<Candidature> Candidatures { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public DbSet<Job> Jobs { get; set; } = default!;
        public DbSet<PaymentPolicy> PaymentPolicies { get; set; }
        public DbSet<Payslip> Payslips { get; set; }
        public DbSet<Admin> Admins { get; set; } = default!;
        public DbSet<SuiviConge> SuiviConges { get; set; } = default!;
        public DbSet<Contrat> Contrats { get; set; } = default!;




    }
}
