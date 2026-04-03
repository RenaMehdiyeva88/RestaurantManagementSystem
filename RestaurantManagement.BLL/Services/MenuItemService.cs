using RestaurantManagement.BLL.Exceptions;
using RestaurantManagement.BLL.Interfafces;
using RestaurantManagement.DAL.Entities;
using RestaurantManagement.DAL.Interfaces;

namespace RestaurantManagement.BLL.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _repository;
        private readonly IRestaurantRepository _restaurantRepository;

        public MenuItemService(IMenuItemRepository repository, IRestaurantRepository restaurantRepository)
        {
            _repository = repository;
            _restaurantRepository = restaurantRepository;
        }

        public async Task<List<MenuItem>> GetByRestaurantAsync(int restaurantId)
            => await _repository.GetByRestaurantIdAsync(restaurantId);

        public async Task<MenuItem> GetByIdAsync(int id)
        {
            var menuItem = await _repository.GetByIdAsync(id);
            if (menuItem == null)
                throw new NotFoundException($"MenuItem with id {id} not found");
            return menuItem;
        }

        public async Task CreateAsync(int restaurantId, string name, decimal price, string category)
        {
            if (price <= 0)
                throw new ValidationException("Price must be > 0");

            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            if (restaurant == null)
                throw new NotFoundException($"Restaurant with id {restaurantId} not found");

            if (await _repository.ExistsByNameAsync(restaurantId, name))
                throw new AlreadyExistsException($"MenuItem with name '{name}' already exists in this restaurant");

            var menuItem = new MenuItem
            {
                Name = name,
                Price = price,
                Category = category,
                RestaurantId = restaurantId,
                TotalSold = 0
            };

            await _repository.AddAsync(menuItem);
        }

        public async Task UpdateAsync(int id, string name, decimal price, string category)
        {
            var menuItem = await GetByIdAsync(id);

            if (price <= 0)
                throw new ValidationException("Price must be > 0");

            if (menuItem.Name != name &&
                await _repository.ExistsByNameAsync(menuItem.RestaurantId, name))
                throw new AlreadyExistsException($"MenuItem with name '{name}' already exists in this restaurant");

            menuItem.Name = name;
            menuItem.Price = price;
            menuItem.Category = category;

            await _repository.UpdateAsync(menuItem);
        }

        public async Task DeleteAsync(int id)
        {
            await GetByIdAsync(id);
            await _repository.DeleteAsync(id);
        }

        public async Task<List<MenuItem>> GetTopSellingAsync(int count)
            => await _repository.GetTopSellingAsync(count);
    }
}
