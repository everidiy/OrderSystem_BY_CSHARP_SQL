using Orders_System_c__sql.Data;
using Orders_System_c__sql.Services;
using Orders_System_c__sql.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace OrdersSystemSql
{
    public partial class AddOrderItem : Window
    {
        public AddOrderItem(
            int orderId,
            ObservableCollection<OrderItem> list,
            ProductViewModel productViewModel)
        {
            InitializeComponent();

            DataContext = new OrderItemViewModel(
                new WindowService(this),
                new DataOrderItem(),
                new OrderItemService(),
                productViewModel,
                orderId);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Hide();
            Application.Current.MainWindow?.Show();
        }
    }
}