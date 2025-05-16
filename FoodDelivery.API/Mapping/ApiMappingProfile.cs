using AutoMapper;
using FoodDelivery.API.Models;
using FoodDelivery.BLL.Models;
using FoodDelivery.DAL.Entities;

namespace FoodDelivery.API.Mapping
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<CategoryDto, CategoryModel>().ReverseMap();
            CreateMap<DishDto, DishModel>().ReverseMap();
            CreateMap<DayOfWeekDto, DayOfWeekModel>().ReverseMap();
            CreateMap<MenuDto, MenuModel>().ReverseMap();
            CreateMap<OrderDto, OrderModel>().ReverseMap();
            CreateMap<OrderItemDto, OrderItemModel>().ReverseMap();
            CreateMap<CreateOrderItemModel, OrderItemDto>();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Dish, DishDto>().ReverseMap();
            CreateMap<DAL.Entities.DayOfWeek, DayOfWeekDto>().ReverseMap();
            CreateMap<Menu, MenuDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        }
    }
}
