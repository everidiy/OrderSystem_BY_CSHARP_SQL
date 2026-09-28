using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Windows;
using Dapper;

namespace OrdersSystemSql
{
    public partial class ViewAllOrders : Window
    {
        public ObservableCollection<OrderWithDetails> OrdersWithDetails { get; set; } = new ObservableCollection<OrderWithDetails>();

        public ViewAllOrders()
        {
            InitializeComponent();

            AllOrdersInfo.ItemsSource = OrdersWithDetails;
            GetAllRows();
        }

        private void GetAllRows()
        {
            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();

                string sqlCommand = """
                    SELECT 
                        o.OrderId,
                        o.CustomerId,
                        o.CreatedAt,
                        o.Status,

                        IFNULL(group_concat(p.Name || ' (x' || oi.Quantity || ')', ', '), 'Null order') AS ProductsList,
                        IFNULL(SUM(p.Price * oi.Quantity), 0) AS TotalPrice

                    FROM Orders o
                    LEFT JOIN OrderItems oi ON o.OrderId = oi.OrderId
                    LEFT JOIN Products p ON oi.ProductId = p.ProductId
                    GROUP BY o.OrderId;
                    """;

                var result = connection.Query<OrderWithDetails>(sqlCommand);

                OrdersWithDetails.Clear();
                foreach (var item in result)
                {
                    OrdersWithDetails.Add(item);
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            Application.Current.MainWindow.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();

            Application.Current.MainWindow.Show();
        }
    }
}
