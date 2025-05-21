using System.Net.Http.Json;

namespace FoodDelivery.ConsoleUI
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static int? currentOrderId = null;
        private static readonly string baseUrl = "https://localhost:7035/api"; // Змініть на вашу URL

        static async Task Main(string[] args)
        {
            client.BaseAddress = new Uri(baseUrl);
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Система доставки їжі ===");
                Console.WriteLine($"Поточне замовлення: {(currentOrderId.HasValue ? $"#{currentOrderId}" : "Не вибрано")}");
                Console.WriteLine("1. Переглянути меню");
                Console.WriteLine("2. Пошук страв");
                Console.WriteLine("3. Переглянути меню за днем тижня");
                Console.WriteLine("4. Переглянути страви за категорією");
                Console.WriteLine("5. Створити нове замовлення");
                Console.WriteLine("6. Додати страву до замовлення");
                Console.WriteLine("7. Переглянути поточне замовлення");
                Console.WriteLine("8. Змінити кількість страви в замовленні");
                Console.WriteLine("9. Видалити страву з замовлення");
                Console.WriteLine("0. Вихід");

                Console.Write("\nВиберіть опцію: ");
                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await ShowAllDishes();
                            break;
                        case "2":
                            await SearchDishes();
                            break;
                        case "3":
                            await ShowMenuByDay();
                            break;
                        case "4":
                            await ShowDishesByCategory();
                            break;
                        case "5":
                            await CreateOrder();
                            break;
                        case "6":
                            await AddDishToOrder();
                            break;
                        case "7":
                            await ShowCurrentOrder();
                            break;
                        case "8":
                            await UpdateOrderItem();
                            break;
                        case "9":
                            await RemoveOrderItem();
                            break;
                        case "0":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                            break;
                    }

                    if (!exit)
                    {
                        Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                    Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
                    Console.ReadKey();
                }
            }
        }

        static async Task ShowAllDishes()
        {
            Console.WriteLine("\n=== Всі доступні страви ===");

            var response = await client.GetAsync("/api/Dishes");
            response.EnsureSuccessStatusCode();

            var dishes = await response.Content.ReadFromJsonAsync<List<DishModel>>();
            DisplayDishes(dishes);
        }

        static async Task SearchDishes()
        {
            Console.Write("\nВведіть назву страви для пошуку: ");
            string searchTerm = Console.ReadLine();

            var response = await client.GetAsync($"/api/Dishes/search?name={Uri.EscapeDataString(searchTerm)}");
            response.EnsureSuccessStatusCode();

            var dishes = await response.Content.ReadFromJsonAsync<List<DishModel>>();

            Console.WriteLine($"\n=== Результати пошуку для '{searchTerm}' ===");
            DisplayDishes(dishes);
        }

        static async Task ShowMenuByDay()
        {
            Console.WriteLine("\n=== Меню за днем тижня ===");
            Console.WriteLine("1. Понеділок");
            Console.WriteLine("2. Вівторок");
            Console.WriteLine("3. Середа");
            Console.WriteLine("4. Четвер");
            Console.WriteLine("5. П'ятниця");
            Console.WriteLine("6. Субота");
            Console.WriteLine("7. Неділя");

            Console.Write("\nВиберіть день тижня (1-7): ");
            if (!int.TryParse(Console.ReadLine(), out int dayId) || dayId < 1 || dayId > 7)
            {
                Console.WriteLine("Невірний ввід. День тижня повинен бути від 1 до 7.");
                return;
            }

            var response = await client.GetAsync($"/api/Menu/day/{dayId}");
            response.EnsureSuccessStatusCode();

            var dishes = await response.Content.ReadFromJsonAsync<List<DishModel>>();

            string[] days = { "Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця", "Субота", "Неділя" };
            Console.WriteLine($"\n=== Меню на {days[dayId - 1]} ===");
            DisplayDishes(dishes);
        }

        static async Task ShowDishesByCategory()
        {
            Console.Write("\nВведіть ID категорії: ");
            if (!int.TryParse(Console.ReadLine(), out int categoryId))
            {
                Console.WriteLine("Невірний ввід. ID категорії повинен бути числом.");
                return;
            }

            var response = await client.GetAsync($"/api/Menu/category/{categoryId}");
            response.EnsureSuccessStatusCode();

            var dishes = await response.Content.ReadFromJsonAsync<List<DishModel>>();

            Console.WriteLine($"\n=== Страви категорії #{categoryId} ===");
            DisplayDishes(dishes);
        }

        static async Task CreateOrder()
        {
            var response = await client.PostAsync("/api/Orders", null);
            response.EnsureSuccessStatusCode();

            var order = await response.Content.ReadFromJsonAsync<OrderModel>();
            currentOrderId = order.Id;

            Console.WriteLine($"\nСтворено нове замовлення з ID: {currentOrderId}");
        }

        static async Task AddDishToOrder()
        {
            if (!currentOrderId.HasValue)
            {
                Console.WriteLine("\nСпочатку створіть замовлення!");
                return;
            }

            Console.Write("\nВведіть ID страви для додавання: ");
            if (!int.TryParse(Console.ReadLine(), out int dishId))
            {
                Console.WriteLine("Невірний ввід. ID страви повинен бути числом.");
                return;
            }

            Console.Write("Введіть кількість: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Невірний ввід. Кількість повинна бути додатнім числом.");
                return;
            }

            var orderItem = new { dishId, quantity };
            var response = await client.PostAsJsonAsync($"/api/Orders/{currentOrderId}/items", orderItem);
            response.EnsureSuccessStatusCode();

            Console.WriteLine($"\nСтраву #{dishId} додано до замовлення в кількості: {quantity}");
        }

        static async Task ShowCurrentOrder()
        {
            if (!currentOrderId.HasValue)
            {
                Console.WriteLine("\nСпочатку створіть замовлення!");
                return;
            }

            var response = await client.GetAsync($"/api/Orders/{currentOrderId}");
            response.EnsureSuccessStatusCode();

            var order = await response.Content.ReadFromJsonAsync<OrderModel>();

            Console.WriteLine($"\n=== Замовлення #{order.Id} ===");
            Console.WriteLine("ID\tСтрава\t\tЦіна\tКількість\tСума");
            Console.WriteLine("--------------------------------------------------");

            foreach (var item in order.OrderItems)
            {
                Console.WriteLine($"{item.Id}\t{item.DishName}\t{item.Price:C}\t{item.Quantity}\t\t{item.TotalPrice:C}");
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Загальна сума: {order.TotalPrice:C}");
        }

        static async Task UpdateOrderItem()
        {
            if (!currentOrderId.HasValue)
            {
                Console.WriteLine("\nСпочатку створіть замовлення!");
                return;
            }

            Console.Write("\nВведіть ID елемента замовлення для оновлення: ");
            if (!int.TryParse(Console.ReadLine(), out int itemId))
            {
                Console.WriteLine("Невірний ввід. ID елемента повинен бути числом.");
                return;
            }

            Console.Write("Введіть нову кількість: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Невірний ввід. Кількість повинна бути додатнім числом.");
                return;
            }

            var orderItem = new { dishId = 0, quantity }; // dishId не використовується при оновленні
            var response = await client.PutAsJsonAsync($"/api/Orders/{currentOrderId}/items/{itemId}", orderItem);
            response.EnsureSuccessStatusCode();

            Console.WriteLine($"\nКількість елемента #{itemId} оновлено до: {quantity}");
        }

        static async Task RemoveOrderItem()
        {
            if (!currentOrderId.HasValue)
            {
                Console.WriteLine("\nСпочатку створіть замовлення!");
                return;
            }

            Console.Write("\nВведіть ID елемента замовлення для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int itemId))
            {
                Console.WriteLine("Невірний ввід. ID елемента повинен бути числом.");
                return;
            }

            var response = await client.DeleteAsync($"/api/Orders/{currentOrderId}/items/{itemId}");
            response.EnsureSuccessStatusCode();

            Console.WriteLine($"\nЕлемент #{itemId} видалено з замовлення");
        }

        static void DisplayDishes(List<DishModel> dishes)
        {
            if (dishes == null || dishes.Count == 0)
            {
                Console.WriteLine("Страви не знайдено.");
                return;
            }

            Console.WriteLine("ID\tНазва\t\tЦіна\tКатегорія\tОпис");
            Console.WriteLine("--------------------------------------------------");

            foreach (var dish in dishes)
            {
                Console.WriteLine($"{dish.Id}\t{dish.Name}\t{dish.Price:C}\t{dish.CategoryName}\t{dish.Description}");
            }
        }
    }
}