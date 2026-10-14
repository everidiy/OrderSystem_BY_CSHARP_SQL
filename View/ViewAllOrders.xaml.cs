using Orders_System_c__sql.Data;
using Orders_System_c__sql.ViewModel;
using System.ComponentModel;
using System.Windows;

namespace OrdersSystemSql
{
    public partial class ViewAllOrders : Window
    {
        public ViewAllOrders()
        {
            InitializeComponent();

            DataContext = new ViewAllOrdersViewModel(
                new DataViewAllOrders(),
                BackWindow);
        }

        private void BackWindow()
        {
            Hide();
            Application.Current.MainWindow?.Show();
        }

        private void Window_Closing(
            object sender,
            CancelEventArgs e)
        {
            e.Cancel = true;

            Hide();
            Application.Current.MainWindow?.Show();
        }
    }
}