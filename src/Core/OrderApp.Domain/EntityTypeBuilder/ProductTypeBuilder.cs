using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Domain.EntityTypeBuilder
{
    public class ProductTypeBuilder : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasMany(i => i.OrderDetails).WithOne(i => i.Product).HasForeignKey(i => i.ProductId);

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnType("int")
                .UseMySqlIdentityColumn<int>();

            builder.Property(p => p.Description)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(1150);
            builder.Property(p => p.Category)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(250);
            builder.Property(p => p.Unit)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(150);
            builder.Property(p => p.UnitPrice)
                 .IsRequired()
                 .HasColumnType("decimal(18,2)")
                 .HasPrecision(18, 2);

        }
    }
}
