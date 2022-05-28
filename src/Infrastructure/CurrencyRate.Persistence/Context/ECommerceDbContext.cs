using OrderApp.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderApp.Domain.Entities;
using OrderApp.Domain.EntityTypeBuilder;
using OrderApp.Domain.Seeds;

namespace OrderApp.Persistence.Context
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new ProductTypeBuilder())
                .ApplyConfiguration(new OrderTypeBuilder())
                .ApplyConfiguration(new OrderDetailTypeBuilder());

            modelBuilder
                .ApplyConfiguration(new ProductSeedData());

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            FillCommonEntity();

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            FillCommonEntity();

            return base.SaveChanges();
        }

        private void FillCommonEntity()
        {
            var datas = ChangeTracker.
               Entries<CommonEntity>();

            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Added:
                        data.Entity.CreateDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        data.Entity.UpdateDate = DateTime.Now;
                        break;
                }
            }

        }
    }
}