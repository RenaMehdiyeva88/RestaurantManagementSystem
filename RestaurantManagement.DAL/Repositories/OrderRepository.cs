using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DAL.Context;
using RestaurantManagement.DAL.Entities;
using RestaurantManagement.DAL.Interfaces;

namespace RestaurantManagement.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly RestaurantDbContext _context;

        public OrderRepository(RestaurantDbContext context) => _context = context;

        public async Task<List<Order>> GetByRestaurantIdAsync(int restaurantId)
            => await _context.Orders
                   .Where(o => o.RestaurantId == restaurantId)
                   .Include(o => o.OrderItems)
                   .ToListAsync();

        public async Task<Order?> GetByIdWithItemsAsync(int id)
            => await _context.Orders
                   .Include(o => o.OrderItems)
                       .ThenInclude(oi => oi.MenuItem)
                   .FirstOrDefaultAsync(o => o.Id == id);

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.Orders.FindAsync(id);
            if (e != null)
            {
                _context.Orders.Remove(e);
                await _context.SaveChangesAsync();
            }
        }
    }
}
