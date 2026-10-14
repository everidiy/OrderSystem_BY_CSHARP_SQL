using OrdersSystemSql;
using System.Collections.ObjectModel;

namespace Orders_System_c__sql.Services
{
    public class OrderItemService
    {
        public ObservableCollection<OrderItem> CurrentOrderItems
        { get; set; } = new ObservableCollection<OrderItem>();
    }
}
