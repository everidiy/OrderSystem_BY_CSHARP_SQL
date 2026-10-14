using Orders_System_c__sql.Models;
using OrdersSystemSql;
using Microsoft.Data.Sqlite;

namespace Orders_System_c__sql.Data
{
    public class DataProduct
    {
        public void Save(Product product)
        {
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
        }
    }
}
