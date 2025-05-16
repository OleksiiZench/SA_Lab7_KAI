using AutoMapper;
using FoodDelivery.API.Models;
using FoodDelivery.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly IDishService _dishService;
        private readonly IMapper _mapper;

        public DishesController(IDishService dishService, IMapper mapper)
        {
            _dishService = dishService;
            _mapper = mapper;
        }

        // GET: api/Dishes
        [HttpGet]
        public ActionResult<IEnumerable<DishModel>> GetDishes()
        {
            var dishes = _dishService.GetAllDishes();
            return Ok(_mapper.Map<List<DishModel>>(dishes));
        }

        // GET: api/Dishes/5
        [HttpGet("{id}")]
        public ActionResult<DishModel> GetDish(int id)
        {
            var dish = _dishService.GetDishById(id);

            if (dish == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DishModel>(dish));
        }

        // GET: api/Dishes/search?name=борщ
        [HttpGet("search")]
        public ActionResult<IEnumerable<DishModel>> SearchDishes(string name)
        {
            var dishes = _dishService.SearchDishesByName(name);
            return Ok(_mapper.Map<List<DishModel>>(dishes));
        }
    }
}
