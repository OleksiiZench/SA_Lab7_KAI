namespace FoodDelivery.API.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
        public decimal TotalPrice { get; set; }
    }
}
