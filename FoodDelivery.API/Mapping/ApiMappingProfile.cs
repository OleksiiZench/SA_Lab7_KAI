using AutoMapper;
using FoodDelivery.API.Models;
using FoodDelivery.BLL.Models;

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
        }
    }
}
