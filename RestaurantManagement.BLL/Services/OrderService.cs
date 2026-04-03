using RestaurantManagement.BLL.Exceptions;
using RestaurantManagement.DAL.Entities;
using RestaurantManagement.DAL.Interfaces;

namespace RestaurantManagement.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ITableRepository _tableRepository;
        private readonly IMenuItemRepository _menuItemRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IRestaurantRepository restaurantRepository,
            ITableRepository tableRepository,
            IMenuItemRepository menuItemRepository)
        {
            _orderRepository = orderRepository;
            _restaurantRepository = restaurantRepository;
            _tableRepository = tableRepository;
            _menuItemRepository = menuItemRepository;
        }

        public async Task<List<Order>> GetByRestaurantAsync(int restaurantId)
        {
            return await _orderRepository.GetByRestaurantIdAsync(restaurantId);
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(id);
            if (order == null)
                throw new NotFoundException($"Order with id {id} not found");

            return order;
        }

        public async Task CreateAsync(int restaurantId, int tableId, List<(int menuItemId, int quantity)> items)
        {
            if (items == null || items.Count == 0)
                throw new ValidationException("Order must contain at least one item");

            //Fetch and validate restaurant
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId)
                ?? throw new NotFoundException($"Restaurant with id {restaurantId} not found");

            //Fetch and validate table
            var table = await _tableRepository.GetByIdAsync(tableId)
                ?? throw new NotFoundException($"Table with id {tableId} not found");

            if (table.RestaurantId != restaurantId)
                throw new ValidationException("Table does not belong to the restaurant");

            var order = new Order
            {
                RestaurantId = restaurantId,
                TableId = tableId,
                OrderDate = DateTime.UtcNow,         
                TotalAmount = 0,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var (menuItemId, quantity) in items)
            {
                if (quantity <= 0)
                    throw new ValidationException("Quantity must be greater than 0");

                var menuItem = await _menuItemRepository.GetByIdAsync(menuItemId)
                    ?? throw new NotFoundException($"MenuItem with id {menuItemId} not found");

                if (menuItem.RestaurantId != restaurantId)
                    throw new ValidationException($"MenuItem with id {menuItemId} does not belong to the restaurant");

                var orderItem = new OrderItem
                {
                    MenuItemId = menuItemId,
                    Quantity = quantity,
                    Price = menuItem.Price
                };

                order.OrderItems.Add(orderItem);
                totalAmount += menuItem.Price * quantity;

                //Update sales count
                menuItem.TotalSold += quantity;
                await _menuItemRepository.UpdateAsync(menuItem);
            }

            order.TotalAmount = totalAmount;

            //Save order
            await _orderRepository.AddAsync(order);

            //Update restaurant statistics
            restaurant.TotalOrders++;
            restaurant.TotalRevenue += totalAmount;
            await _restaurantRepository.UpdateAsync(restaurant);

            //Update table statistics
            table.OrderCount++;
            await _tableRepository.UpdateAsync(table);
        }

        public async Task UpdateAsync(int orderId, int tableId, List<(int menuItemId, int quantity)> items)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(orderId)
                ?? throw new NotFoundException($"Order with id {orderId} not found");

            order.TableId = tableId;
            order.OrderItems.Clear();

            decimal totalAmount = 0;

            foreach (var (menuItemId, quantity) in items)
            {
                if (quantity <= 0)
                    throw new ValidationException("Quantity must be greater than 0");

                var menuItem = await _menuItemRepository.GetByIdAsync(menuItemId);
                if (menuItem == null || menuItem.RestaurantId != order.RestaurantId)
                    throw new NotFoundException($"MenuItem with id {menuItemId} not found or does not belong to restaurant");

                var orderItem = new OrderItem
                {
                    MenuItemId = menuItemId,
                    Quantity = quantity,
                    Price = menuItem.Price
                };

                order.OrderItems.Add(orderItem);
                totalAmount += menuItem.Price * quantity;
            }

            order.TotalAmount = totalAmount;
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteAsync(int id)
        {
            //This will throw NotFoundException if order doesn't exist
            await GetByIdAsync(id);
            await _orderRepository.DeleteAsync(id);
        }
    }
}