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
    public class OrderTypeBuilder : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasMany(i => i.OrderDetails).WithOne(i => i.Order).HasForeignKey(i => i.OrderId);

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnType("int")
                .UseMySqlIdentityColumn<int>();

            builder.Property(p => p.CustomerName)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(1150);
            builder.Property(p => p.CustomerEmail)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(250);
            builder.Property(p => p.CustomerGSM)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(150);
            builder.Property(p => p.TotalAmount)
                 .IsRequired()
                 .HasColumnType("decimal(18,2)")
                 .HasPrecision(18, 2);

        }
    }
}
