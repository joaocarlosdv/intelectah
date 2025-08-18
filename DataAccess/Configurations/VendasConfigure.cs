using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class VendasConfigure : IEntityTypeConfiguration<Vendas>
    {
        public void Configure(EntityTypeBuilder<Vendas> builder)
        {
            builder.ToTable("Vendas");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            builder.HasOne(x => x.Veiculo)
                   .WithMany(y => y.ListaVendas)
                   .HasForeignKey(x => x.VeiculoId);

            builder.HasOne(x => x.Concessionaria)
                   .WithMany(y => y.ListaVendas)
                   .HasForeignKey(x => x.ConcessionariaId);

            builder.HasOne(x => x.Cliente)
                   .WithMany(y => y.ListaVendas)
                   .HasForeignKey(x => x.ClienteId);
        }
    }
}
