using RestaurantManagement.DAL.Entities;

public interface IOrderService
{
    Task<List<Order>> GetByRestaurantAsync(int restaurantId);
    Task<Order> GetByIdAsync(int id);

    Task CreateAsync(int restaurantId, int tableId,
                     List<(int menuItemId, int quantity)> items);

    Task UpdateAsync(int orderId, int tableId,
                     List<(int menuItemId, int quantity)> items);

    Task DeleteAsync(int id);
}