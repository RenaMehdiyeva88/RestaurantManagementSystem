using RestaurantManagement.BLL.Exceptions;
using RestaurantManagement.BLL.Interfafces;
using RestaurantManagement.BLL.Services;
using RestaurantManagement.DAL.Entities;

namespace RestaurantManagement.UI;

public static class MenuOperations
{
    public static async Task Run(
        IRestaurantService restaurantService,
        ITableService tableService,
        IMenuItemService menuItemService,
        IOrderService orderService)
    {
        while (true)
        {
            PrintMenu();
            var choice = ReadInput("Enter your choice");

            switch (choice)
            {
                case "1":
                    await RestaurantMenu(restaurantService);
                    break;
                case "2":
                    await TableMenu(tableService, restaurantService);
                    break;
                case "3":
                    await MenuItemMenu(menuItemService, restaurantService);
                    break;
                case "4":
                    await OrderMenu(orderService, restaurantService, tableService, menuItemService);
                    break;
                case "5":
                    await ViewRestaurantStatus(restaurantService);
                    break;
                case "6":
                    await ViewRestaurantRevenue(restaurantService);
                    break;
                case "7":
                    await ViewTopSellingItems(menuItemService, restaurantService);
                    break;
                case "0":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine("\n=== RESTAURANT MANAGEMENT ===");
        Console.WriteLine("--- DATA ENTRY ---");
        Console.WriteLine("  1) Manage Restaurants");
        Console.WriteLine("  2) Manage Tables");
        Console.WriteLine("  3) Manage Menu Items");
        Console.WriteLine("  4) Manage Orders");
        Console.WriteLine("--- REPORTS ---");
        Console.WriteLine("  5) View Restaurant Status");
        Console.WriteLine("  6) View Revenue Report");
        Console.WriteLine("  7) View Top Selling Items");
        Console.WriteLine("  0) Exit");
        Console.WriteLine("===============================");
    }

    //Helper Methods
    private static string ReadInput(string prompt)
    {
        Console.Write($"{prompt}: ");
        return Console.ReadLine() ?? "";
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt}: ");
            if (int.TryParse(Console.ReadLine() ?? "0", out int result))
                return result;
            PrintError("Please enter a valid number");
        }
    }

    private static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt}: ");
            if (decimal.TryParse(Console.ReadLine() ?? "0", out decimal result))
                return result;
            PrintError("Please enter a valid decimal number");
        }
    }

    private static void PrintHeader(string text)
    {
        Console.WriteLine($"\n===== {text} =====");
    }
    private static void PrintSuccess(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    private static void PrintError(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✗ {text}");
        Console.ResetColor();
    }

    private static void PrintTableHeader(string title, string header)
    {
        PrintHeader(title);
        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));
    }

    //Restaurant Menu
    private static async Task RestaurantMenu(IRestaurantService restaurantService)
    {
        while (true)
        {
            PrintHeader("Restaurant Management");
            Console.WriteLine("1. Add Restaurant");
            Console.WriteLine("2. View All Restaurants");
            Console.WriteLine("3. View Restaurant by ID");
            Console.WriteLine("4. Update Restaurant");
            Console.WriteLine("5. Delete Restaurant");
            Console.WriteLine("0. Back to Main Menu");

            int choice = ReadInt("Choose an option");
            if (choice == 0) break;

            try
            {
                switch (choice)
                {
                    case 1:
                        await AddRestaurant(restaurantService);
                        break;
                    case 2:
                        await ViewAllRestaurants(restaurantService);
                        break;
                    case 3:
                        await ViewRestaurantById(restaurantService);
                        break;
                    case 4:
                        await UpdateRestaurant(restaurantService);
                        break;
                    case 5:
                        await DeleteRestaurant(restaurantService);
                        break;
                    default:
                        PrintError("Invalid choice.");
                        break;
                }
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }
        }
    }

    private static async Task AddRestaurant(IRestaurantService service)
    {
        Console.WriteLine("\n--- Add Restaurant ---");
        var name = ReadInput("Restaurant Name");
        var code = ReadInt("Branch Code (1-99)");

        await service.CreateAsync(name, code);
        PrintSuccess("Restaurant added successfully.");
    }

    private static async Task ViewAllRestaurants(IRestaurantService service)
    {
        var restaurants = await service.GetAllAsync();
        PrintTableHeader("All Restaurants", $"{"ID",5} {"Name",-25} {"Code",6} {"Total Orders",15} {"Revenue",12}");

        if (!restaurants.Any())
        {
            Console.WriteLine("No restaurants found.");
            return;
        }

        foreach (var r in restaurants)
            Console.WriteLine($"{r.Id,5} {r.Name,-25} {r.BranchCode,6} {r.TotalOrders,15} {r.TotalRevenue,12:C}");
    }

    private static async Task ViewRestaurantById(IRestaurantService service)
    {
        var id = ReadInt("Enter Restaurant ID");
        var restaurant = await service.GetByIdAsync(id);

        PrintHeader("Restaurant Details");
        Console.WriteLine($"ID: {restaurant.Id}");
        Console.WriteLine($"Name: {restaurant.Name}");
        Console.WriteLine($"Branch Code: {restaurant.BranchCode}");
        Console.WriteLine($"Total Orders: {restaurant.TotalOrders}");
        Console.WriteLine($"Total Revenue: {restaurant.TotalRevenue:C}");
        Console.WriteLine($"Active Tables: {restaurant.ActiveTables}");
    }

    private static async Task UpdateRestaurant(IRestaurantService service)
    {
        Console.WriteLine("\n--- Update Restaurant ---");
        var id = ReadInt("Enter Restaurant ID");
        var name = ReadInput("New Restaurant Name");
        var code = ReadInt("New Branch Code");

        await service.UpdateAsync(id, name, code);
        PrintSuccess("Restaurant updated successfully.");
    }

    private static async Task DeleteRestaurant(IRestaurantService service)
    {
        Console.WriteLine("\n--- Delete Restaurant ---");
        var id = ReadInt("Enter Restaurant ID");

        await service.DeleteAsync(id);
        PrintSuccess("Restaurant deleted successfully.");
    }

    //Table Menu
    private static async Task TableMenu(ITableService tableService, IRestaurantService restaurantService)
    {
        while (true)
        {
            PrintHeader("Table Management");
            Console.WriteLine("1. Add Table");
            Console.WriteLine("2. View Tables by Restaurant");
            Console.WriteLine("3. View Table by ID");
            Console.WriteLine("4. Update Table");
            Console.WriteLine("5. Delete Table");
            Console.WriteLine("0. Back to Main Menu");

            int choice = ReadInt("Choose an option");
            if (choice == 0) break;

            try
            {
                switch (choice)
                {
                    case 1:
                        await AddTable(tableService, restaurantService);
                        break;
                    case 2:
                        await ViewTablesByRestaurant(tableService, restaurantService);
                        break;
                    case 3:
                        await ViewTableById(tableService);
                        break;
                    case 4:
                        await UpdateTable(tableService);
                        break;
                    case 5:
                        await DeleteTable(tableService);
                        break;
                    default:
                        PrintError("Invalid choice.");
                        break;
                }
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }

        }
    }

    private static async Task AddTable(ITableService service, IRestaurantService restaurantService)
    {
        Console.WriteLine("\n--- Add Table ---");
        var restaurantId = ReadInt("Restaurant ID");
        var tableNumber = ReadInt("Table Number");
        var capacity = ReadInt("Capacity");

        await service.CreateAsync(restaurantId, tableNumber, capacity);
        PrintSuccess("Table added successfully.");
    }

    private static async Task ViewTablesByRestaurant(ITableService service, IRestaurantService restaurantService)
    {
        var restaurantId = ReadInt("Enter Restaurant ID");
        var tables = await service.GetByRestaurantAsync(restaurantId);

        PrintTableHeader("Tables", $"{"ID",5} {"Table #",10} {"Capacity",10} {"Orders",10}");

        if (!tables.Any())
        {
            Console.WriteLine("No tables found.");
            return;
        }

        foreach (var t in tables)
            Console.WriteLine($"{t.Id,5} {t.TableNumber,10} {t.Capacity,10} {t.OrderCount,10}");
    }

    private static async Task ViewTableById(ITableService service)
    {
        var id = ReadInt("Enter Table ID");
        var table = await service.GetByIdAsync(id);

        PrintHeader("Table Details");
        Console.WriteLine($"ID: {table.Id}");
        Console.WriteLine($"Table Number: {table.TableNumber}");
        Console.WriteLine($"Capacity: {table.Capacity}");
        Console.WriteLine($"Restaurant ID: {table.RestaurantId}");
        Console.WriteLine($"Order Count: {table.OrderCount}");
    }

    private static async Task UpdateTable(ITableService service)
    {
        Console.WriteLine("\n--- Update Table ---");
        var id = ReadInt("Enter Table ID");
        var tableNumber = ReadInt("New Table Number");
        var capacity = ReadInt("New Capacity");

        await service.UpdateAsync(id, tableNumber, capacity);
        PrintSuccess("Table updated successfully.");
    }

    private static async Task DeleteTable(ITableService service)
    {
        Console.WriteLine("\n--- Delete Table ---");
        var id = ReadInt("Enter Table ID");

        await service.DeleteAsync(id);
        PrintSuccess("Table deleted successfully.");
    }

    //Menu Item Menu

    private static async Task MenuItemMenu(IMenuItemService menuItemService, IRestaurantService restaurantService)
    {
        while (true)
        {
            PrintHeader("Menu Items Management");
            Console.WriteLine("1. Add Menu Item");
            Console.WriteLine("2. View Items by Restaurant");
            Console.WriteLine("3. View Item by ID");
            Console.WriteLine("4. Update Item");
            Console.WriteLine("5. Delete Item");
            Console.WriteLine("0. Back to Main Menu");

            int choice = ReadInt("Choose an option");
            if (choice == 0) break;

            try
            {
                switch (choice)
                {
                    case 1:
                        await AddMenuItem(menuItemService, restaurantService);
                        break;
                    case 2:
                        await ViewMenuItemsByRestaurant(menuItemService, restaurantService);
                        break;
                    case 3:
                        await ViewMenuItemById(menuItemService);
                        break;
                    case 4:
                        await UpdateMenuItem(menuItemService);
                        break;
                    case 5:
                        await DeleteMenuItem(menuItemService);
                        break;
                    default:
                        PrintError("Invalid choice.");
                        break;
                }
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }
        }
    }

    private static async Task AddMenuItem(IMenuItemService service, IRestaurantService restaurantService)
    {
        Console.WriteLine("\n--- Add Menu Item ---");
        var restaurantId = ReadInt("Restaurant ID");
        var name = ReadInput("Item Name");
        var price = ReadDecimal("Price");
        var category = ReadInput("Category");

        await service.CreateAsync(restaurantId, name, price, category);
        PrintSuccess("Menu item added successfully.");
    }

    private static async Task ViewMenuItemsByRestaurant(IMenuItemService service, IRestaurantService restaurantService)
    {
        var restaurantId = ReadInt("Enter Restaurant ID");
        var items = await service.GetByRestaurantAsync(restaurantId);

        PrintTableHeader("Menu Items", $"{"ID",5} {"Name",-25} {"Price",10} {"Category",-15} {"Sold",8}");

        if (!items.Any())
        {
            Console.WriteLine("No menu items found.");
            return;
        }

        foreach (var item in items)
            Console.WriteLine($"{item.Id,5} {item.Name,-25} {item.Price,10:C} {item.Category,-15} {item.TotalSold,8}");
    }

    private static async Task ViewMenuItemById(IMenuItemService service)
    {
        var id = ReadInt("Enter Menu Item ID");
        var item = await service.GetByIdAsync(id);

        PrintHeader("Menu Item Details");
        Console.WriteLine($"ID: {item.Id}");
        Console.WriteLine($"Name: {item.Name}");
        Console.WriteLine($"Price: {item.Price:C}");
        Console.WriteLine($"Category: {item.Category}");
        Console.WriteLine($"Restaurant ID: {item.RestaurantId}");
        Console.WriteLine($"Total Sold: {item.TotalSold}");
    }

    private static async Task UpdateMenuItem(IMenuItemService service)
    {
        Console.WriteLine("\n--- Update Menu Item ---");
        var id = ReadInt("Enter Menu Item ID");
        var name = ReadInput("New Name");
        var price = ReadDecimal("New Price");
        var category = ReadInput("New Category");

        await service.UpdateAsync(id, name, price, category);
        PrintSuccess("Menu item updated successfully.");
    }

    private static async Task DeleteMenuItem(IMenuItemService service)
    {
        Console.WriteLine("\n--- Delete Menu Item ---");
        var id = ReadInt("Enter Menu Item ID");

        await service.DeleteAsync(id);
        PrintSuccess("Menu item deleted successfully.");
    }

    //Order Menu

    private static async Task OrderMenu(
        IOrderService orderService,
        IRestaurantService restaurantService,
        ITableService tableService,
        IMenuItemService menuItemService)
    {
        while (true)
        {
            PrintHeader("Order Management");
            Console.WriteLine("1. Create New Order");
            Console.WriteLine("2. View Order by ID");
            Console.WriteLine("3. Update Order");
            Console.WriteLine("4. Delete Order");
            Console.WriteLine("5. List Orders by Restaurant");
            Console.WriteLine("0. Back to Main Menu");

            int choice = ReadInt("Choose an option");
            if (choice == 0) break;

            try
            {
                switch (choice)
                {
                    case 1:
                        await CreateOrder(orderService, restaurantService, tableService, menuItemService);
                        break;
                    case 2:
                        await ViewOrderById(orderService);
                        break;
                    case 3:
                        await UpdateOrder(orderService, tableService, menuItemService);
                        break;
                    case 4:
                        await DeleteOrder(orderService);
                        break;
                    case 5:
                        await ViewOrdersByRestaurant(orderService, restaurantService);
                        break;
                    default:
                        PrintError("Invalid choice.");
                        break;
                }
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }

        }
    }

    private static async Task CreateOrder(
        IOrderService orderService,
        IRestaurantService restaurantService,
        ITableService tableService,
        IMenuItemService menuItemService)
    {
        Console.WriteLine("\n--- Create New Order ---");
        int restaurantId = ReadInt("Enter Restaurant ID");
        int tableId = ReadInt("Enter Table ID");

        var items = new List<(int menuItemId, int quantity)>();

        Console.WriteLine("\nAdd order items (enter 0 to finish):");

        while (true)
        {
            int menuItemId = ReadInt("Menu Item ID (0 to finish)");
            if (menuItemId == 0) break;

            int quantity = ReadInt("Quantity");

            items.Add((menuItemId, quantity));
            Console.WriteLine($"   ✓ Added: Item {menuItemId} × {quantity}");
        }

        if (items.Count == 0)
        {
            PrintError("Order must contain at least one item.");
            return;
        }

        await orderService.CreateAsync(restaurantId, tableId, items);
        PrintSuccess("Order created successfully!");
    }

    private static async Task ViewOrderById(IOrderService orderService)
    {
        int orderId = ReadInt("Enter Order ID");
        var order = await orderService.GetByIdAsync(orderId);

        PrintHeader("Order Details");
        Console.WriteLine($"Order #{order.Id}");
        Console.WriteLine($"Restaurant ID: {order.RestaurantId}");
        Console.WriteLine($"Table ID: {order.TableId}");
        Console.WriteLine($"Date: {order.OrderDate}");
        Console.WriteLine($"Total Amount: {order.TotalAmount:C}");

        Console.WriteLine("\nOrder Items:");
        Console.WriteLine($"{"Item ID",10} {"Quantity",12} {"Price",12}");
        Console.WriteLine(new string('-', 34));
        foreach (var item in order.OrderItems)
        {
            Console.WriteLine($"{item.MenuItemId,10} {item.Quantity,12} {item.Price,12:C}");
        }
    }

    private static async Task UpdateOrder(
    IOrderService orderService,
    ITableService tableService,
    IMenuItemService menuItemService)
    {
        Console.WriteLine("\n--- Update Order ---");

        int orderId = ReadInt("Enter Order ID");
        int tableId = ReadInt("Enter New Table ID");

        var items = new List<(int menuItemId, int quantity)>();

        Console.WriteLine("\nAdd order items (enter 0 to finish):");

        while (true)
        {
            int menuItemId = ReadInt("Menu Item ID (0 to finish)");
            if (menuItemId == 0)
                break;

            int quantity = ReadInt("Quantity");

            items.Add((menuItemId, quantity));
            Console.WriteLine($"✓ Added: Item {menuItemId} × {quantity}");
        }

        if (items.Count == 0)
        {
            PrintError("Order must contain at least one item.");
            return;
        }

        await orderService.UpdateAsync(orderId, tableId, items);

        PrintSuccess("Order updated successfully!");
    }

    private static async Task DeleteOrder(IOrderService orderService)
    {
        Console.WriteLine("\n--- Delete Order ---");

        int orderId = ReadInt("Enter Order ID");

        await orderService.DeleteAsync(orderId);

        PrintSuccess("Order deleted successfully.");
    }

    private static async Task ViewOrdersByRestaurant(
     IOrderService orderService,
     IRestaurantService restaurantService)
    {
        int restaurantId = ReadInt("Enter Restaurant ID");

        var orders = await orderService.GetByRestaurantAsync(restaurantId);

        PrintTableHeader(
            "Orders",
            $"{"ID",5} {"Table",8} {"Date",22} {"Total",12}");

        if (!orders.Any())
        {
            Console.WriteLine("No orders found.");
            return;
        }

        foreach (var o in orders)
        {
            Console.WriteLine(
                $"{o.Id,5} {o.TableId,8} {o.OrderDate,22} {o.TotalAmount,12:C}");
        }
    }

    //Reports

    private static async Task ViewRestaurantStatus(IRestaurantService service)
    {
        var id = ReadInt("\nEnter Restaurant ID");
        var restaurant = await service.GetByIdAsync(id);

        PrintTableHeader("RESTAURANT STATUS", $"{"Name",-20} {"Orders",10} {"Revenue",15} {"Active Tables",15}");
        Console.WriteLine($"{restaurant.Name,-20} {restaurant.TotalOrders,10} {restaurant.TotalRevenue,15:C} {restaurant.ActiveTables,15}");
    }

    private static async Task ViewRestaurantRevenue(IRestaurantService service)
    {
        var restaurants = await service.GetOrderedByRevenueAsync();

        PrintTableHeader("REVENUE REPORT", $"{"Name",-25} {"Total Orders",15} {"Revenue",15}");

        if (!restaurants.Any())
        {
            Console.WriteLine("No restaurants found.");
            return;
        }

        foreach (var r in restaurants)
        {
            Console.WriteLine($"{r.Name,-25} {r.TotalOrders,15} {r.TotalRevenue,15:C}");
        }
    }

    private static async Task ViewTopSellingItems(IMenuItemService menuItemService, IRestaurantService restaurantService)
    {
        var items = await menuItemService.GetTopSellingAsync(5);

        PrintTableHeader("TOP SELLING ITEMS", $"{"Name",-25} {"Price",12} {"Category",-15} {"Sold",8}");

        if (!items.Any())
        {
            Console.WriteLine("No items found.");
            return;
        }

        foreach (var item in items)
        {
            Console.WriteLine($"{item.Name,-25} {item.Price,12:C} {item.Category,-15} {item.TotalSold,8}");
        }
    }
}