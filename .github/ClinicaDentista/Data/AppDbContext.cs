using Microsoft.EntityFrameworkCore;
using ClinicaDentista.Models;

namespace ClinicaDentista.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
    }
}
