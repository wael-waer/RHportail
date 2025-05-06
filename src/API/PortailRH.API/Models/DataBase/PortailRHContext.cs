

namespace PortailRH.API.Models.DataBase
{
    public class PortailRHContext:DbContext
    {
        public PortailRHContext(DbContextOptions<PortailRHContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = default!;
        public DbSet<Conge> Conges { get; set; } = default!;
        public DbSet<Candidature> Candidatures { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public DbSet<Job> Jobs { get; set; } = default!;




    }
}
