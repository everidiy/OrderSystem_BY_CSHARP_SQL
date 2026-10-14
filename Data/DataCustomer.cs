using Microsoft.Data.Sqlite;
using OrdersSystemSql;

namespace Orders_System_c__sql.Data
{
    public class DataCustomer
    {
        public void Save(Customer customer)
        {
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
        }
    }
}
