using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace OrdersSystemSql
{
    public partial class AddOrderItem : Window
    {
        private int orderIdForItem = 0;
        public ObservableCollection<OrderItem> CurrentOrderItems { get; set; } =
            new ObservableCollection<OrderItem>();

        public AddOrderItem(int orderId, ObservableCollection<OrderItem> list)
        {
            InitializeComponent();

            orderIdForItem = orderId;
            CurrentOrderItems = list;
            ProductsContainer.ItemsSource = MainWindow.products;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            MainWindow.createNewOrder.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();

            Application.Current.MainWindow.Show();
        }

        private void ProductsContainer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsContainer.SelectedItem is Product selectedProduct)
            {
                ItemQuantity_Value.Text = selectedProduct.Stock.ToString();
            }
            else
            {
                ItemQuantity_Value.Text = "0";
            }
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!(ProductsContainer.SelectedItem is Product selectedProduct))
            {
                MessageBox.Show("Пожалуйста, выберите товар из списка!");
                return;
            }

            if (!int.TryParse(ItemQuantityForOrder_Value.Text, out int orderQuantity) || orderQuantity <= 0)
            {
                MessageBox.Show("Введите корректное количество товара (положительное число)!");
                return;
            }

            if (selectedProduct.Stock == 0)
            {
                MessageBox.Show("Товар полностью отсутствует на складе!");
                return;
            }

            if (orderQuantity > selectedProduct.Stock)
            {
                MessageBox.Show($"Превышен лимит склада! Доступно всего: {selectedProduct.Stock}");
                return;
            }

            selectedProduct.Stock -= orderQuantity;
            ItemQuantity_Value.Text = selectedProduct.Stock.ToString();

            OrderItem orderItem = new OrderItem()
            {
                OrderId = orderIdForItem,
                ProductId = selectedProduct.ProductId,
                Quantity = orderQuantity,
                Product = selectedProduct
            };

            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();

                string sqlCommand = """
                    INSERT INTO OrderItems (OrderId, ProductId, Quantity)
                    VALUES (@orderId, @productId, @quantity);
                    SELECT last_insert_rowid(); 
                    """;

                using (var command = new SqliteCommand(sqlCommand, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderItem.OrderId);
                    command.Parameters.AddWithValue("@productId", orderItem.ProductId);
                    command.Parameters.AddWithValue("@quantity", orderItem.Quantity);
                    orderItem.OrderItemId = Convert.ToInt32(command.ExecuteScalar());
                }

                string updateSqlCommand = """
                    UPDATE Products
                    SET Stock = @stock
                    WHERE ProductId = @productId
                    """;

                using (var command = new SqliteCommand(updateSqlCommand, connection))
                {
                    command.Parameters.AddWithValue("@stock", selectedProduct.Stock);
                    command.Parameters.AddWithValue("@productId", selectedProduct.ProductId);
                    command.ExecuteNonQuery();
                }
            }

            CurrentOrderItems.Add(orderItem);
            this.Hide();
        }

    }
}
