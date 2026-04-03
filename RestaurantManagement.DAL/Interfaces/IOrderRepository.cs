using RestaurantManagement.DAL.Entities;

namespace RestaurantManagement.DAL.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetByRestaurantIdAsync(int restaurantId);
        Task<Order?> GetByIdWithItemsAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
    }
}
