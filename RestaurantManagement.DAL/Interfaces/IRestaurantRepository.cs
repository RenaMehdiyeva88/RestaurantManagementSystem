using RestaurantManagement.DAL.Entities;

namespace RestaurantManagement.DAL.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<List<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetByIdAsync(int id);
        Task AddAsync(Restaurant restaurant);
        Task UpdateAsync(Restaurant restaurant);
        Task DeleteAsync(int id);
        Task<List<Restaurant>> GetOrderedByRevenueAsync();
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByBranchCodeAsync(int branchCode);
    }
}
