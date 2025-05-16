using AutoMapper;
using FoodDelivery.API.Models;
using FoodDelivery.BLL.Models;
using FoodDelivery.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        // POST: api/Orders
        [HttpPost]
        public ActionResult<OrderModel> CreateOrder()
        {
            var newOrder = _orderService.CreateOrder();
            return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, _mapper.Map<OrderModel>(newOrder));
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public ActionResult<OrderModel> GetOrder(int id)
        {
            var orderItems = _orderService.GetOrderItems(id);

            if (orderItems == null || !orderItems.Any())
            {
                return NotFound();
            }

            var totalPrice = _orderService.CalculateTotalOrderPrice(id);

            var orderModel = new OrderModel
            {
                Id = id,
                OrderItems = _mapper.Map<List<OrderItemModel>>(orderItems),
                TotalPrice = totalPrice
            };

            return Ok(orderModel);
        }

        // POST: api/Orders/5/items
        [HttpPost("{id}/items")]
        public ActionResult AddDishToOrder(int id, [FromBody] CreateOrderItemModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            _orderService.AddDishToOrder(id, model.DishId, model.Quantity);
            return NoContent();
        }

        // GET: api/Orders/5/items
        [HttpGet("{id}/items")]
        public ActionResult<IEnumerable<OrderItemModel>> GetOrderItems(int id)
        {
            var orderItems = _orderService.GetOrderItems(id);

            if (orderItems == null || !orderItems.Any())
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<OrderItemModel>>(orderItems));
        }

        // DELETE: api/Orders/5/items/3
        [HttpDelete("{orderId}/items/{itemId}")]
        public ActionResult DeleteOrderItem(int orderId, int itemId)
        {
            // Тут потрібно додати метод до сервісу для видалення елемента замовлення
            // _orderService.RemoveOrderItem(orderId, itemId);

            // Оскільки цього методу немає в наданому коді, повернемо NotImplemented
            return StatusCode(501, "Метод не реалізований");
        }

        // PUT: api/Orders/5/items/3
        [HttpPut("{orderId}/items/{itemId}")]
        public ActionResult UpdateOrderItem(int orderId, int itemId, [FromBody] CreateOrderItemModel model)
        {
            // Тут потрібно додати метод до сервісу для оновлення елемента замовлення
            // _orderService.UpdateOrderItem(orderId, itemId, model.Quantity);

            // Оскільки цього методу немає в наданому коді, повернемо NotImplemented
            return StatusCode(501, "Метод не реалізований");
        }
    }
}
