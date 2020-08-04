using Application.DTOs;
using Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Application.POCOs
{
    public class ProductPoco : IValidatableObject
    {
        public ProductDto Data { get; set; }

        public ProductPoco()
        {
            this.Data = new ProductDto();
        }

        public ProductPoco(ProductDto dto) : base()
        {
            this.Data = dto;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            var productService = (IProductService)(validationContext.GetService(typeof(IProductService)));

            if (!IsValidName(Data.Name, productService))
            {
                results.Add(new ValidationResult("Le nom du produit existe déjà."));
            }

            return results;
        }

        public bool IsValidName(string productName, IProductReadableService productService)
        {
            var allProduct = productService
                .GetProducts()
                .GetAwaiter()
                .GetResult();

            return allProduct.Count(
                product =>
                {
                    return product.Name.Equals(productName, StringComparison.InvariantCultureIgnoreCase);
                }).Equals(0);
        }
    }
}
