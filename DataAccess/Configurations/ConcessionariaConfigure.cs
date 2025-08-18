using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ConcessionariaConfigure : IEntityTypeConfiguration<Concessionaria>
    {
        public void Configure(EntityTypeBuilder<Concessionaria> builder)
        {
            builder.ToTable("Concessionaria");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
        }
    }
}
