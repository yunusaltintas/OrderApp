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
    public class OrderDetailTypeBuilder :IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnType("int")
                .UseMySqlIdentityColumn<int>();

            builder.Property(p => p.UnitPrice)
                 .IsRequired()
                 .HasColumnType("decimal(18,2)")
                 .HasPrecision(18, 2);

        }
    }
}
