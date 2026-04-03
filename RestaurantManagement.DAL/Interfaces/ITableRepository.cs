using RestaurantManagement.DAL.Entities;

namespace RestaurantManagement.DAL.Interfaces
{
    public interface ITableRepository
    {
        Task<List<Table>> GetByRestaurantIdAsync(int restaurantId);
        Task<Table?> GetByIdAsync(int id);
        Task AddAsync(Table table);
        Task UpdateAsync(Table table);
        Task DeleteAsync(int id);
        Task<bool> ExistsByTableNumberAsync(int restaurantId, int tableNumber);
    }
}
