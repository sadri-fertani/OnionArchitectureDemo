using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Product, ProductDto>()
                .ReverseMap();
        }
    }
}
