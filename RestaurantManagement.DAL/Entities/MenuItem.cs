using System.Diagnostics.Contracts;

namespace RestaurantManagement.DAL.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } //unique per restaurant
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int RestaurantId { get; set; }
        public int TotalSold { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
