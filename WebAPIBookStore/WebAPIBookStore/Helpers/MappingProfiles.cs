using AutoMapper;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<User, UserLogin>().ReverseMap();
            CreateMap<CartItem, CartItemCreate>().ReverseMap();
        }
    }
}
