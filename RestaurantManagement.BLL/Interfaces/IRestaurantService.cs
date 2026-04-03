using RestaurantManagement.DAL.Entities;

public interface IRestaurantService
{
    Task<List<Restaurant>> GetAllAsync();
    Task<Restaurant> GetByIdAsync(int id);
    Task CreateAsync(string name, int branchCode);
    Task UpdateAsync(int id, string name, int branchCode);
    Task DeleteAsync(int id);
    Task<List<Restaurant>> GetOrderedByRevenueAsync();
    Task<List<Restaurant>> GetStatusAsync(); //statistics ucun
}