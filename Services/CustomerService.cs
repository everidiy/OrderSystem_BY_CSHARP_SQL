using Orders_System_c__sql.Models;
using OrdersSystemSql;
using System.Collections.ObjectModel;

namespace Orders_System_c__sql.Services
{
    public static class CustomerService
    {
        public static ObservableCollection<Customer> Customers { get; set; }
            = new ObservableCollection<Customer>();
    }
}