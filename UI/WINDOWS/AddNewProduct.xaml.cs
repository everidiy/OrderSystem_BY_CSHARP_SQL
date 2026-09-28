using Microsoft.Data.Sqlite;
using System.Windows;

namespace OrdersSystemSql
{
    public partial class AddNewProduct : Window
    {

        public AddNewProduct()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();

            Application.Current.MainWindow.Show();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            Application.Current.MainWindow.Show();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product()
            {
                Name = ProductName_Value.Text,
                Price = double.Parse(ProductPrice_Value.Text),
                Stock = int.Parse(ProductQuantity_Value.Text)
            };

            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();

                string sqlCommand = """
                    INSERT INTO Products (Name, Price, Stock)
                    VALUES (@name, @price, @stock);

                    SELECT last_insert_rowid();
                    """;

                using (var command = new SqliteCommand(sqlCommand, connection))
                {
                    command.Parameters.AddWithValue("@name", product.Name);
                    command.Parameters.AddWithValue("@price", product.Price);
                    command.Parameters.AddWithValue("@stock", product.Stock);

                    product.ProductId = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            MainWindow.products.Add(product);

            ProductName_Value.Text = string.Empty;
            ProductPrice_Value.Text = string.Empty;
            ProductQuantity_Value.Text = string.Empty;
        }
    }
}
