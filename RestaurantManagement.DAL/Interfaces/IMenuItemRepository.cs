using RestaurantManagement.DAL.Entities;

namespace RestaurantManagement.DAL.Interfaces
{
    public interface IMenuItemRepository
    {
        Task<List<MenuItem>> GetByRestaurantIdAsync(int restaurantId);
        Task<MenuItem?> GetByIdAsync(int id);
        Task AddAsync(MenuItem menuItem);
        Task UpdateAsync(MenuItem menuItem);
        Task DeleteAsync(int id);
        Task<bool> ExistsByNameAsync(int restaurantId, string name);
        Task<List<MenuItem>> GetTopSellingAsync(int count);
    }
}
