namespace FoodDelivery.API.Models
{
    public class OrderItemModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int DishId { get; set; }
        public DishModel Dish { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
