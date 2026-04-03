using Microsoft.EntityFrameworkCore;
using RestaurantManagement.DAL.Context;
using RestaurantManagement.DAL.Entities;
using RestaurantManagement.DAL.Interfaces;

namespace RestaurantManagement.DAL.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly RestaurantDbContext _context;

        public TableRepository(RestaurantDbContext context) => _context = context;

        public async Task<List<Table>> GetByRestaurantIdAsync(int restaurantId)
            => await _context.Tables.Where(t => t.RestaurantId == restaurantId).ToListAsync();

        public async Task<Table?> GetByIdAsync(int id) => await _context.Tables.FindAsync(id);

        public async Task AddAsync(Table table)
        {
            await _context.Tables.AddAsync(table);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Table table)
        {
            _context.Tables.Update(table);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var e = await _context.Tables.FindAsync(id);
            if (e != null)
            {
                _context.Tables.Remove(e);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsByTableNumberAsync(int restaurantId, int tableNumber)
            => await _context.Tables.AnyAsync(
                   t => t.RestaurantId == restaurantId && t.TableNumber == tableNumber);
    }
}
