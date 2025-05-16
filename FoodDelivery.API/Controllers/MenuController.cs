using AutoMapper;
using FoodDelivery.API.Models;
using FoodDelivery.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public MenuController(IMenuService menuService, IMapper mapper)
        {
            _menuService = menuService;
            _mapper = mapper;
        }

        // GET: api/Menu/day/5
        [HttpGet("day/{dayOfWeekId}")]
        public ActionResult<IEnumerable<DishModel>> GetMenuForDay(int dayOfWeekId)
        {
            if (dayOfWeekId < 1 || dayOfWeekId > 7)
            {
                return BadRequest("День тижня повинен бути від 1 до 7");
            }

            var dishes = _menuService.GetMenuForDay(dayOfWeekId);
            return Ok(_mapper.Map<List<DishModel>>(dishes));
        }

        // GET: api/Menu/category/2
        [HttpGet("category/{categoryId}")]
        public ActionResult<IEnumerable<DishModel>> GetDishesByCategory(int categoryId)
        {
            var dishes = _menuService.GetDishesByCategory(categoryId);
            return Ok(_mapper.Map<List<DishModel>>(dishes));
        }
    }
}
