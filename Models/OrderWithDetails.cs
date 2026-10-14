using OrdersSystemSql;

namespace Orders_System_c__sql.Models
{
    public class OrderWithDetails : Order
    {
        public string ProductsList { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
