using DataAccess.Configurations;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        public DbSet<Fabricante> Fabricante { get; set; }
        public DbSet<Veiculo> Veiculo { get; set; }
        public DbSet<Concessionaria> Concessionaria { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Vendas> Vendas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FabricanteConfigure());
            modelBuilder.ApplyConfiguration(new VeiculoConfigure());
            modelBuilder.ApplyConfiguration(new ConcessionariaConfigure());
            modelBuilder.ApplyConfiguration(new ClienteConfigure());
            modelBuilder.ApplyConfiguration(new VendasConfigure());
            modelBuilder.ApplyConfiguration(new UsuarioConfigure());

            base.OnModelCreating(modelBuilder);
        }
    }
}
