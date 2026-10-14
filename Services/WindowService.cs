using OrdersSystemSql;
using System.Windows;
using System.ComponentModel;

namespace Orders_System_c__sql.Services
{
    public class WindowService : IWindowService
    {
        private readonly Window window;

        public WindowService(Window window)
        {
            this.window = window;
            window.Closing += Window_Closing;
        }

        public void OpenAddNewCustomer()
        {
            OpenWindow(new AddNewCustomer());
        }

        public void OpenAddNewProduct()
        {
            OpenWindow(new AddNewProduct());
        }

        public void OpenCreateNewOrder()
        {
            OpenWindow(new CreateNewOrder());
        }

        public void OpenViewAllOrders()
        {
            OpenWindow(new ViewAllOrders());
        }

        public void BackWindow()
        {
            OpenWindow(new MainWindow());
        }

        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;

            window.Hide();
            Application.Current.MainWindow.Show();
        }

        private void OpenWindow(Window window1)
        {
            window1.Hide();

            window.Closed += (sender, e) =>
            {
                window.Show();
            };
            
            window1.Show();
        }
    }
}