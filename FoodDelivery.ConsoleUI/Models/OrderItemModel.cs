public class OrderItemModel
{
    public int Id { get; set; }
    public int DishId { get; set; }
    public string DishName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}