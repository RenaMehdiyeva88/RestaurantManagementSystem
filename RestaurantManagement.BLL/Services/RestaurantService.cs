using RestaurantManagement.BLL.Exceptions;
using RestaurantManagement.DAL.Entities;
using RestaurantManagement.DAL.Interfaces;

namespace RestaurantManagement.BLL.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _repository;

        public RestaurantService(IRestaurantRepository repository) => _repository = repository;

        public async Task<List<Restaurant>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<Restaurant> GetByIdAsync(int id)
        {
            var restaurant = await _repository.GetByIdAsync(id);
            if (restaurant == null)
                throw new NotFoundException($"Restaurant with id {id} not found");
            return restaurant;
        }

        public async Task CreateAsync(string name, int branchCode)
        {
            if (branchCode < 1 || branchCode > 99)
                throw new ValidationException("BranchCode must be in range 1-99");

            if (await _repository.ExistsByNameAsync(name))
                throw new AlreadyExistsException($"Restaurant with name '{name}' already exists");

            if (await _repository.ExistsByBranchCodeAsync(branchCode))
                throw new AlreadyExistsException($"Restaurant with branch code {branchCode} already exists");

            var restaurant = new Restaurant
            {
                Name = name,
                BranchCode = branchCode,
                TotalOrders = 0,
                TotalRevenue = 0,
                ActiveTables = 0
            };

            await _repository.AddAsync(restaurant);
        }

        public async Task UpdateAsync(int id, string name, int branchCode)
        {
            var restaurant = await GetByIdAsync(id);

            if (restaurant.Name != name && await _repository.ExistsByNameAsync(name))
                throw new AlreadyExistsException($"Restaurant with name '{name}' already exists");

            if (restaurant.BranchCode != branchCode && await _repository.ExistsByBranchCodeAsync(branchCode))
                throw new AlreadyExistsException($"Restaurant with branch code {branchCode} already exists");

            if (branchCode < 1 || branchCode > 99)
                throw new ValidationException("BranchCode must be in range 1-99");

            restaurant.Name = name;
            restaurant.BranchCode = branchCode;

            await _repository.UpdateAsync(restaurant);
        }

        public async Task DeleteAsync(int id)
        {
            await GetByIdAsync(id);
            await _repository.DeleteAsync(id);
        }

        public async Task<List<Restaurant>> GetOrderedByRevenueAsync()
            => await _repository.GetOrderedByRevenueAsync();

        public async Task<List<Restaurant>> GetStatusAsync()
            => await _repository.GetAllAsync();
    }
}
