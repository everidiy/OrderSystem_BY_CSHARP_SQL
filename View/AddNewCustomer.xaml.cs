using Microsoft.Data.Sqlite;
using Orders_System_c__sql.Data;
using Orders_System_c__sql.Services;
using Orders_System_c__sql.ViewModel;
using System.Windows;

namespace OrdersSystemSql
{
    public partial class AddNewCustomer : Window
    {

        public AddNewCustomer()
        {
            InitializeComponent();

            DataContext = new CustomerViewModel(new WindowService(this), new DataCustomer());
        }
    }
}
