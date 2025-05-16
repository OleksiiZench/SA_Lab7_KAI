namespace FoodDelivery.API.Models
{
    public class MenuModel
    {
        public int Id { get; set; }
        public int DayOfWeekId { get; set; }
        public DayOfWeekModel DayOfWeek { get; set; }
        public List<DishModel> Dishes { get; set; } = new List<DishModel>();
    }
}
