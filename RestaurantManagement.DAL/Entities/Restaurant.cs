namespace RestaurantManagement.DAL.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } //unique
        public int BranchCode { get; set; } //1-99, unique
        public int TotalOrders { get; set; } //default : 0
        public decimal TotalRevenue { get; set; } //default : 0
        public int ActiveTables { get; set; } //default : 0
        public ICollection<Table> Tables { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
