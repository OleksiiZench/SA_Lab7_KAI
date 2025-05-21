public class OrderModel
{
    public int Id { get; set; }
    public List<OrderItemModel> OrderItems { get; set; }
    public decimal TotalPrice { get; set; }
}