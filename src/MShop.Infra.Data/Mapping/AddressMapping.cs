using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MShop.Domain.ValueObjects;

namespace Mshop.Infra.Data.Mapping
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");

            builder.HasKey(a=>a.Id); // Se Address não tiver Id, remova esta linha

            builder.Property(a => a.Street).IsRequired().HasColumnType("varchar(100)");
            builder.Property(a => a.Number).IsRequired().HasColumnType("varchar(10)");
            builder.Property(a => a.Complement).HasColumnType("varchar(50)");
            builder.Property(a => a.District).IsRequired().HasColumnType("varchar(50)");
            builder.Property(a => a.City).IsRequired().HasColumnType("varchar(50)");
            builder.Property(a => a.State).IsRequired().HasColumnType("varchar(50)");
            builder.Property(a => a.PostalCode).IsRequired().HasColumnType("varchar(20)");
            builder.Property(a => a.Country).IsRequired().HasColumnType("varchar(50)");

            builder.Ignore(c => c.Events);
        }
    }
}