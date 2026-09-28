namespace OrdersSystemSql
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}
