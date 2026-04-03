using RestaurantManagement.DAL.Entities;

namespace RestaurantManagement.BLL.Interfafces
{
    public interface ITableService
    {
        Task<List<Table>> GetByRestaurantAsync(int restaurantId);
        Task<Table> GetByIdAsync(int id);
        Task CreateAsync(int restaurantId, int tableNumber, int capacity);
        Task UpdateAsync(int id, int tableNumber, int capacity);
        Task DeleteAsync(int id);
    }
}
