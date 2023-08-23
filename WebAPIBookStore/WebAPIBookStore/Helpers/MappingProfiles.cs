using AutoMapper;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Input;
using WebAPIBookStore.Models;
using WebAPIBookStore.Result;

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
            CreateMap<User, UserOutput>().ReverseMap();
            CreateMap<User, SignUpInput>().ReverseMap();
            CreateMap<User, UpdateUserInput>().ReverseMap();
            CreateMap<Product, ProductOutput>().ReverseMap();
        }
    }
}
