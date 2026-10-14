using Orders_System_c__sql.Services;
using Orders_System_c__sql.ViewModel;
using System.Windows;

namespace OrdersSystemSql
{
    public partial class MainWindow : Window
    {
        public static AddNewCustomer customer;
        public static AddNewProduct product;
        public static CreateNewOrder createNewOrder;
        public static ViewAllOrders viewAll;

        public MainWindow()
        {
            SQLitePCL.Batteries.Init();

            InitializeComponent();

            Database.Initialize();
            DataContext = new MainViewModel(new WindowService(this));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}