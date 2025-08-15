using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MShop.Domain.Entities;

namespace Mshop.Infra.Data.Mapping
{
    public class CustomerMapping : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.Email)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(c => c.Phone)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(c => c.Password)
                .IsRequired()
                .HasColumnType("varchar(255)");

            /*builder.Property(c => c.CreatedInKeycloak)
                .IsRequired()
                .HasColumnType("bool")
                .HasDefaultValue(false);*/

            builder.Property(c => c.AddressId);
                
            builder.Ignore(c => c.Events);
            builder.Ignore(c => c.Address);
        }
    }
}