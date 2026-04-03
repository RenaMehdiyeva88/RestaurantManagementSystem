namespace RestaurantManagement.DAL.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public int TableNumber { get; set; } //unique per restaurant
        public int Capacity { get; set; }
        public int RestaurantId { get; set; }
        public int OrderCount { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
