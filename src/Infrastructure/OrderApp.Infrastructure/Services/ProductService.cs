using Microsoft.EntityFrameworkCore;
using OrderApp.Application.Dtos;
using OrderApp.Application.Dtos.BaseResponse;
using OrderApp.Application.Interfaces.IRepository;
using OrderApp.Application.Interfaces.IService;
using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseRepository<Product> _productRepository;

        public ProductService(IBaseRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetProductsAsync(string? categoryName)
        {
            var result = _productRepository.Query();

            if (!string.IsNullOrEmpty(categoryName))
                result = result.Where(p => p.Category == categoryName);

            return await result.ToListAsync();
        }
    }
}
