using Microsoft.EntityFrameworkCore;
using ProjetoMoveisMedeiros.Api.Models;

namespace ProjetoMoveisMedeiros.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Obra> Obras { get; set; }
        public DbSet<ItemChecklistObra> ItensChecklistObra { get; set; }
        public DbSet<NaoConformidade> NaoConformidades { get; set; }

    }
}