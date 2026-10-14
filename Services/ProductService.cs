using Orders_System_c__sql.Models;
using System.Collections.ObjectModel;

namespace Orders_System_c__sql.Services
{
    public static class ProductService
    {
        public static ObservableCollection<Product> Products { get; set; } = 
            new ObservableCollection<Product>();
    }
}
