using Application.DTOs;
using Application.Interfaces.Persistences;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public ProductService(
            IMapper mapper,
            IUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ProductDto> GetProduct(long id)
        {
            var currentProduct = await _uow.GetRepository<Product>().GetAsync(id);

            return _mapper.Map<ProductDto>(currentProduct);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var currentProducts = await _uow.GetRepository<Product>().GetAsync();

            return _mapper.Map<IEnumerable<ProductDto>>(currentProducts);
        }

        public async Task<ProductDto> InsertProduct(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            _uow.GetRepository<Product>().Add(product);

            await _uow.CommitAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateProduct(ProductDto dto)
        {
            var oldProduct = await _uow.GetRepository<Product>().GetAsync(dto.Id);
            if (oldProduct == null) throw new ArgumentException($"Product with id {dto.Id} unfound");

            var product = _mapper.Map<Product>(dto);

            _uow.GetRepository<Product>().Update(product);

            await _uow.CommitAsync();
        }

        public async Task DeleteProduct(long id)
        {
            var product = await _uow.GetRepository<Product>().GetAsync(id);
            if (product == null) throw new ArgumentException($"Product with id {id} unfound");

            _uow.GetRepository<Product>().Delete(product);

            await _uow.CommitAsync();
        }
    }
}
