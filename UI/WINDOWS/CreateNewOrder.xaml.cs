using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.VisualBasic;

namespace OrdersSystemSql
{

    public partial class CreateNewOrder : Window
    {
        public ObservableCollection<OrderItem> CurrentOrderItems { get; set; } = 
            new ObservableCollection<OrderItem>();

        public CreateNewOrder()
        {
            InitializeComponent();
            CustomersContainer.ItemsSource = MainWindow.customers;

            OrderItemsContainer.ItemsSource = CurrentOrderItems;
        }

        private int orderIdFor = 0;
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (orderIdFor == 0)
            {
                int customerId = 0;
                if (CustomersContainer.SelectedItem is Customer selectedCustomer)
                {
                    customerId = selectedCustomer.CustomerId;
                }

                if (customerId == 0)
                {
                    MessageBox.Show("Choose customer, please!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var connection = new SqliteConnection(Database.ConnectionString))
                {
                    connection.Open();
                    string sqlCommand = """
                        INSERT INTO Orders (CustomerId, CreatedAt, Status)
                        VALUES (@customerId, @createdAt, @status);
                        SELECT last_insert_rowid();
                        """;

                    using (var command = new SqliteCommand(sqlCommand, connection))
                    {
                        command.Parameters.AddWithValue("@customerId", customerId);
                        command.Parameters.AddWithValue("@createdAt", DateTime.Now);
                        command.Parameters.AddWithValue("@status", "New");

                        orderIdFor = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }

            AddOrderItem addOrderItem = new AddOrderItem(orderIdFor, CurrentOrderItems);
            addOrderItem.ShowDialog();

            double total = 0;

            foreach (OrderItem item in CurrentOrderItems)
            {
                foreach (Product product in MainWindow.products)
                {
                    if (item.ProductId == product.ProductId)
                    {
                        total += product.Price * item.Quantity;
                    }
                }
            }

            TotalPrice.Text = total.ToString("N2");
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string @string = Interaction.InputBox
                ("Enter product number, which you want to delete:", "Delete Product", "1");

            if (string.IsNullOrEmpty(@string))
            {
                MessageBox.Show("Deletion cancelled.", "Cancelled", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!int.TryParse(@string, out int productNumber) || productNumber <= 0)
            {
                MessageBox.Show("Please enter a valid positive number!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var itemToRemove = CurrentOrderItems.FirstOrDefault(item => item.ProductId == productNumber);

            if (itemToRemove != null)
            {
                var product = MainWindow.products.FirstOrDefault(p => p.ProductId == productNumber);

                using (var connection = new SqliteConnection(Database.ConnectionString))
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string deleteSql = """
                                DELETE FROM OrderItems 
                                WHERE OrderId = @orderId AND ProductId = @productId
                                """;

                            using (var command = new SqliteCommand(deleteSql, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@orderId", orderIdFor);
                                command.Parameters.AddWithValue("@productId", productNumber);
                                command.ExecuteNonQuery();
                            }

                            string updateStockSql = """
                                UPDATE Products 
                                SET Stock = Stock + @quantity 
                                WHERE ProductId = @productId
                                """;

                            using (var command = new SqliteCommand(updateStockSql, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@quantity", itemToRemove.Quantity);
                                command.Parameters.AddWithValue("@productId", productNumber);
                                command.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                if (product != null)
                {
                    product.Stock += itemToRemove.Quantity;
                }

                CurrentOrderItems.Remove(itemToRemove);

                double total = 0;

                foreach (OrderItem item in CurrentOrderItems)
                {
                    foreach (Product prod in MainWindow.products)
                    {
                        if (prod.ProductId == item.ProductId)
                        {
                            total += prod.Price * item.Quantity;
                            break;
                        }
                    }
                }

                TotalPrice.Text = total.ToString("N2");

                MessageBox.Show($"Product #{productNumber} has been successfully deleted from database and order.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Product #{productNumber} not found in your order.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (orderIdFor == 0 || CurrentOrderItems.Count == 0)
            {
                MessageBox.Show("Your order is null. Add some products, please!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int customerId = 0;
            if (CustomersContainer.SelectedItem is Customer selectedCustomer)
            {
                customerId = selectedCustomer.CustomerId;
            }

            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();
                string sqlCommand = """
                    UPDATE Orders 
                    SET CustomerId = @customerId 
                    WHERE OrderId = @orderId
                    """;

                using (var command = new SqliteCommand(sqlCommand, connection))
                {
                    command.Parameters.AddWithValue("@customerId", customerId);
                    command.Parameters.AddWithValue("@orderId", orderIdFor);
                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Your order successfule saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
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
