using RestaurantManagement.DAL.Entities;

namespace RestaurantManagement.BLL.Interfafces
{
    public interface IMenuItemService
    {
        Task<List<MenuItem>> GetByRestaurantAsync(int restaurantId);
        Task<MenuItem> GetByIdAsync(int id);
        Task CreateAsync(int restaurantId, string name, decimal price, string category);
        Task UpdateAsync(int id, string name, decimal price, string category);
        Task DeleteAsync(int id);
        Task<List<MenuItem>> GetTopSellingAsync(int count);
    }
}
