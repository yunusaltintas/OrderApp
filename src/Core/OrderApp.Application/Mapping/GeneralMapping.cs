using AutoMapper;
using OrderApp.Application.Dtos;
using OrderApp.Application.Dtos.Requests;
using OrderApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Mapping
{
    public class GeneralMapping : Profile
    {

        public GeneralMapping()
        {
            CreateMap<Product, ProductDto>()
                .ReverseMap();

            CreateMap<OrderDetail, ProductDetail>()
                .ReverseMap();

            CreateMap<CreateOrderRequest, Order>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.ProductDetails))
                .ForMember(dest=>dest.TotalAmount, opt=>opt.MapFrom(u=>u.ProductDetails.Count));

        }
    }
}
