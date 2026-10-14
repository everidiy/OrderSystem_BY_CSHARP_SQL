using Orders_System_c__sql.Services;
using Orders_System_c__sql.ViewModel;
using Orders_System_c__sql.Data;
using System.Windows;

namespace OrdersSystemSql
{
    public partial class AddNewProduct : Window
    {

        public AddNewProduct()
        {
            InitializeComponent();
            
            DataContext = new ProductViewModel(new WindowService(this), new DataProduct());
        }
    }
}
