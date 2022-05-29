using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderApp.Application.Dtos;
using OrderApp.Application.Dtos.BaseResponse;
using OrderApp.Application.Interfaces.IService;
using OrderApp.Domain.Entities;
using System.Net;

namespace OrderApp.API.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        public ProductController(IProductService productService, IMapper mapper, ICacheService cache)
        {
            _productService = productService;
            _mapper = mapper;
            _cache = cache;
        }
        [HttpGet]
        public async Task<ActionResult<CustomResponseDto<List<ProductDto>>>> GetProducts(string? category)
        {
            string cacheKey = "Products" + category;
            var resultList = await _cache.GetOrAddAsync<List<Product>>(cacheKey, async () => await _productService.GetProductsAsync(category));

            if (resultList is null)
                return new NoContentResult();

            return new OkObjectResult(
                new CustomResponseDto<List<ProductDto>>().Success((int)HttpStatusCode.OK, _mapper.Map<List<ProductDto>>(resultList)));
        }

    }
}
