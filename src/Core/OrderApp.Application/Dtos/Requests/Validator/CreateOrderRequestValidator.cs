using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Application.Dtos.Requests.Validator
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(o => o.CustomerName).NotEmpty().NotNull();
            RuleFor(o => o.CustomerGSM).NotEmpty().NotNull();
            RuleFor(o => o.CustomerEmail).EmailAddress().NotEmpty().NotNull();
        }
    }

    public class ProductDetailValitator : AbstractValidator<ProductDetail>
    {
        public ProductDetailValitator()
        {
            RuleFor(o => o.UnitPrice).NotNull();
            RuleFor(o => o.ProductId).NotNull();
            RuleFor(o => o.Amount).NotNull();
        }
    }
}
