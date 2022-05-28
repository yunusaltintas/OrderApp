using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderApp.Application.Dtos;
using OrderApp.Application.Dtos.BaseResponse;
using OrderApp.Application.Interfaces.IService;
using System.Net;

namespace OrderApp.API.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<CustomResponseDto<List<ProductDto>>>> GetProducts(string? category)
        {
            var resultList =await _productService.GetProductsAsync(category);

            if (resultList is null)
                return new NoContentResult();

            return new OkObjectResult(
                new CustomResponseDto<List<ProductDto>>().Success((int)HttpStatusCode.OK, _mapper.Map<List<ProductDto>>(resultList)));
        }

    }
}
