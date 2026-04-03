using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DAL.Context;
using RestaurantManagement.DAL.Entities;
using RestaurantManagement.DAL.Interfaces;

namespace RestaurantManagement.DAL.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantDbContext _context;
        public RestaurantRepository(RestaurantDbContext context) => _context = context;
        public async Task<List<Restaurant>> GetAllAsync()
        => await _context.Restaurants.ToListAsync();
        public async Task<Restaurant?> GetByIdAsync(int id)
        => await _context.Restaurants.FindAsync(id);
        public async Task AddAsync(Restaurant restaurant)
        {
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var e = await _context.Restaurants.FindAsync(id);
            if (e != null) { _context.Restaurants.Remove(e); await _context.SaveChangesAsync(); }
        }
        public async Task<List<Restaurant>> GetOrderedByRevenueAsync()
        => await _context.Restaurants.OrderByDescending(r => r.TotalRevenue).ToListAsync();
        public async Task<bool> ExistsByNameAsync(string name)
        => await _context.Restaurants.AnyAsync(r => r.Name == name);
        public async Task<bool> ExistsByBranchCodeAsync(int branchCode)
        => await _context.Restaurants.AnyAsync(r => r.BranchCode == branchCode);
    }
}
