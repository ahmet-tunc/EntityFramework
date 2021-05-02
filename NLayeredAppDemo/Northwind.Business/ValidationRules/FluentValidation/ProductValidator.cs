using FluentValidation;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Business.ValidationRules.FluentValidation
{
    public class ProductValidator:AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.ProductName).NotEmpty().WithMessage("Ürün adı boş geçilemez");
            RuleFor(p => p.QuantityPerUnit).NotEmpty().WithMessage("Birim fiyatı boş geçilemez");
            RuleFor(p => p.UnitPrice).NotEmpty().WithMessage("Ürün fiyatı boş geçilemez");
            RuleFor(p => p.UnitsInStock).NotEmpty().WithMessage("Stok adeti boş geçilemez");
            RuleFor(p => p.UnitPrice).GreaterThan(0);
            RuleFor(p => p.ProductName).Must(StartWithA);
        }

        private bool StartWithA(string arg)
        {
            return arg.StartsWith("A");
        }
    }
}
