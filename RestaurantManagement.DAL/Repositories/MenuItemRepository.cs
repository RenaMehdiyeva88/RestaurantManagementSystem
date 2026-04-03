using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DAL.Context;
using RestaurantManagement.DAL.Entities;
using RestaurantManagement.DAL.Interfaces;

namespace RestaurantManagement.DAL.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly RestaurantDbContext _context;

        public MenuItemRepository(RestaurantDbContext context) => _context = context;

        public async Task<List<MenuItem>> GetByRestaurantIdAsync(int restaurantId)
            => await _context.MenuItems.Where(m => m.RestaurantId == restaurantId).ToListAsync();

        public async Task<MenuItem?> GetByIdAsync(int id) => await _context.MenuItems.FindAsync(id);

        public async Task AddAsync(MenuItem menuItem)
        {
            await _context.MenuItems.AddAsync(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MenuItem menuItem)
        {
            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.MenuItems.FindAsync(id);
            if (e != null)
            {
                _context.MenuItems.Remove(e);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<MenuItem>> GetTopSellingAsync(int count)
            => await _context.MenuItems
                   .OrderByDescending(m => m.TotalSold)
                   .Take(count)
                   .ToListAsync();

        public async Task<bool> ExistsByNameAsync(int restaurantId, string name)
            => await _context.MenuItems.AnyAsync(
                   m => m.RestaurantId == restaurantId && m.Name == name);
    }
}
