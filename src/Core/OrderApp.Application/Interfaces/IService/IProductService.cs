using OrderApp.Application.Dtos;
using OrderApp.Application.Dtos.BaseResponse;
using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Interfaces.IService
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync(string? categoryName);
    }
}
