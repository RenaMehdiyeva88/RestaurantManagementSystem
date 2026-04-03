using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantManagement.BLL.Interfafces;
using RestaurantManagement.BLL.Services;
using RestaurantManagement.DAL.Context;
using RestaurantManagement.DAL.Interfaces;
using RestaurantManagement.DAL.Repositories;

namespace RestaurantManagement.UI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = ConfigureServices();

        using var scope = serviceProvider.CreateScope();

        var restaurantService = scope.ServiceProvider.GetRequiredService<IRestaurantService>();
        var tableService = scope.ServiceProvider.GetRequiredService<ITableService>();
        var menuItemService = scope.ServiceProvider.GetRequiredService<IMenuItemService>();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        await MenuOperations.Run(restaurantService, tableService, menuItemService, orderService);
    }

    private static ServiceProvider ConfigureServices()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();

        // Database
        services.AddDbContext<RestaurantDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Services
        services.AddScoped<IRestaurantService, RestaurantService>();
        services.AddScoped<ITableService, TableService>();
        services.AddScoped<IMenuItemService, MenuItemService>();
        services.AddScoped<IOrderService, OrderService>();

        return services.BuildServiceProvider();
    }
}