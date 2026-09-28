using System.Collections.ObjectModel;
using System.Windows;

namespace OrdersSystemSql
{
    public partial class MainWindow : Window
    {
        public static AddNewCustomer customer;
        public static AddNewProduct product;
        public static CreateNewOrder createNewOrder;
        public static ViewAllOrders viewAll;

        public static ObservableCollection<Customer> customers { get; set; }
        public static ObservableCollection<Product> products { get; set; }
        public static ObservableCollection<Order> orders { get; set; }

        public MainWindow()
        {
            SQLitePCL.Batteries.Init();

            InitializeComponent();

            Database.Initialize();

            customers = new ObservableCollection<Customer>();
            products = new ObservableCollection<Product>();
            orders = new ObservableCollection<Order>();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void AddNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            customer = new AddNewCustomer();
            customer.Show();

            this.Hide();
        }

        private void AddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            product = new AddNewProduct();
            product.Show();
            
            this.Hide();
        }

        private void CreateNewOrder_Click(object sender, RoutedEventArgs e)
        {
            createNewOrder = new CreateNewOrder();
            createNewOrder.Show();

            this.Hide();
        }

        private void ViewAllOrders_Click(object sender, RoutedEventArgs e)
        {
            viewAll = new ViewAllOrders();
            viewAll.Show();

            this.Hide();
        }
    }
}