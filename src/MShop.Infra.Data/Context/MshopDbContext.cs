using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MShop.Infra.Data.Mapping;
using MShop.Domain.Entities;
using MShop.Domain.ValueObjects;

namespace MShop.Infra.Data.Context
{
    public class MshopDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public MshopDbContext(DbContextOptions<MshopDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach(var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties())
                .Where(p => p.ClrType == typeof(string)))
            {
                property.SetColumnType("Varchar(100)");
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MshopDbContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientNoAction;
                //relationship.IsOwnership = false;
                //relationship.DeclaringEntityType.RemoveForeignKey(relationship);

            }

            base.OnModelCreating(modelBuilder);
        }
    }
}