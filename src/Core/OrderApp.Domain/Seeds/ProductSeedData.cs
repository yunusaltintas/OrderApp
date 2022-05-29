using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Domain.Seeds
{
    public class ProductSeedData : IEntityTypeConfiguration<Product>
    {
        public string[] Description { get; set; }
        public string[] Category { get; set; }
        public string Unit { get; set; }
        public decimal[] UnitPrice { get; set; }
        public bool[] Status { get; set; }
        public List<Product> Products { get; set; }

        public ProductSeedData()
        {
            this.Products = new List<Product>();
            this.Description = new string[3] { "Giyilir", "Yenir", "Kulanılır" };
            this.Category = new string[3] { "Kıyafet", "Yumurta", "Bardak" };
            this.Unit = "4141";
            this.UnitPrice = new decimal[3] { 41, 57, 51 };
            this.Status = new bool[3] { true, false, true };

            Random random = new Random();

            for (int i = 1; i < 1000; i++)
            {
                int index = random.Next(3);

                Products.Add(new Product
                {
                    Id = i,
                    Description = this.Description[index],
                    Category = this.Category[index],
                    Unit = this.Unit,
                    UnitPrice = this.UnitPrice[index],
                    Status = this.Status[index],
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                });
            }
        }
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(Products);
        }
    }
}
