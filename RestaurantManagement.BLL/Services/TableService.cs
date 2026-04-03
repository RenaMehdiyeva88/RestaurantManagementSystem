using RestaurantManagement.BLL.Exceptions;
using RestaurantManagement.BLL.Interfafces;
using RestaurantManagement.DAL.Entities;
using RestaurantManagement.DAL.Interfaces;

namespace RestaurantManagement.BLL.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;
        private readonly IRestaurantRepository _restaurantRepository;

        public TableService(ITableRepository tableRepository, IRestaurantRepository restaurantRepository)
        {
            _tableRepository = tableRepository;
            _restaurantRepository = restaurantRepository;
        }

        public async Task<List<Table>> GetByRestaurantAsync(int restaurantId)
            => await _tableRepository.GetByRestaurantIdAsync(restaurantId);

        public async Task<Table> GetByIdAsync(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            if (table == null)
                throw new NotFoundException($"Table with id {id} not found");
            return table;
        }

        public async Task CreateAsync(int restaurantId, int tableNumber, int capacity)
        {
            if (capacity <= 0)
                throw new ValidationException("Capacity must be > 0");

            if (await _tableRepository.ExistsByTableNumberAsync(restaurantId, tableNumber))
                throw new AlreadyExistsException($"Table with number {tableNumber} already exists in this restaurant");

            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null)
                throw new NotFoundException($"Restaurant with id {restaurantId} not found");

            var table = new Table
            {
                TableNumber = tableNumber,
                Capacity = capacity,
                RestaurantId = restaurantId,
                OrderCount = 0
            };

            await _tableRepository.AddAsync(table);

            restaurant.ActiveTables++;
            await _restaurantRepository.UpdateAsync(restaurant);
        }

        public async Task UpdateAsync(int id, int tableNumber, int capacity)
        {
            var table = await GetByIdAsync(id);

            if (capacity <= 0)
                throw new ValidationException("Capacity must be > 0");

            if (table.TableNumber != tableNumber &&
                await _tableRepository.ExistsByTableNumberAsync(table.RestaurantId, tableNumber))
                throw new AlreadyExistsException($"Table with number {tableNumber} already exists in this restaurant");

            table.TableNumber = tableNumber;
            table.Capacity = capacity;

            await _tableRepository.UpdateAsync(table);
        }

        public async Task DeleteAsync(int id)
        {
            var table = await GetByIdAsync(id);
            await _tableRepository.DeleteAsync(id);

            var restaurant = await _restaurantRepository.GetByIdAsync(table.RestaurantId);
            if (restaurant != null && restaurant.ActiveTables > 0)
            {
                restaurant.ActiveTables--;
                await _restaurantRepository.UpdateAsync(restaurant);
            }
        }
    }
}
