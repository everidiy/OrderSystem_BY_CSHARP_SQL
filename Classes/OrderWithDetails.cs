namespace OrdersSystemSql
{
    public class OrderWithDetails : Order
    {
        public string ProductsList { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
