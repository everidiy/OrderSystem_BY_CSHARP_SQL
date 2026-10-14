using Orders_System_c__sql.Data;
using Orders_System_c__sql.Services;
using Orders_System_c__sql.ViewModel;
using System.ComponentModel;
using System.Windows;

namespace OrdersSystemSql
{
    public partial class CreateNewOrder : Window
    {
        private readonly NewOrderService newOrderService;

        private readonly IWindowService windowService;
        private readonly DataProduct data;

        public CreateNewOrder()
        {
            InitializeComponent();

            this.windowService = windowService;
            this.data = data;

            newOrderService = new NewOrderService();

            DataContext = new NewOrderViewModel(
                new DataNewOrder(),
                newOrderService,
                OpenAddItemWindow,
                CloseOrderWindow);
        }

        private void OpenAddItemWindow(int orderId)
        {
            var productViewModel = new ProductViewModel(new WindowService(this), new DataProduct());

            var window = new AddOrderItem(
                orderId,
                newOrderService.CurrentOrderItems,
                productViewModel);

            window.Owner = this;
            window.ShowDialog();
        }

        private void CloseOrderWindow()
        {
            Close();
        }

        private void Window_Closing(
            object sender,
            CancelEventArgs e)
        {
            if (Application.Current.MainWindow != null &&
                Application.Current.MainWindow != this)
            {
                Application.Current.MainWindow.Show();
            }
        }
    }
}