using Microsoft.Data.Sqlite;
using System.Windows;

namespace OrdersSystemSql
{
    public partial class AddNewCustomer : Window
    {

        public AddNewCustomer()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            Application.Current.MainWindow.Show();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = new Customer()
            {
                Name = CustomerName_Value.Text,
                Email = CustomerEmail_Value.Text,
                Phone = CustomerPhone_Value.Text,
            };

            using (var connection = new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();

                string sqlCommand = """
                    INSERT INTO Customers (Name, Email, Phone)
                    VALUES (@name, @email, @phone);

                    SELECT last_insert_rowid();
                    """;

                using (var command = new SqliteCommand(sqlCommand, connection))
                {
                    command.Parameters.AddWithValue("@name", customer.Name);
                    command.Parameters.AddWithValue("@email", customer.Email);
                    command.Parameters.AddWithValue("@phone", customer.Phone);

                    customer.CustomerId = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            MainWindow.customers.Add(customer);

            CustomerName_Value.Text = string.Empty;
            CustomerEmail_Value.Text = string.Empty;
            CustomerPhone_Value.Text = string.Empty;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();

            Application.Current.MainWindow.Show();
        }
    }
}
