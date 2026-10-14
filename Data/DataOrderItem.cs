using Microsoft.Data.Sqlite;
using Orders_System_c__sql.Models;
using OrdersSystemSql;

namespace Orders_System_c__sql.Data
{
    public class DataOrderItem
    {
        public void Save(OrderItem item, Product selectedProduct)
        {
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
                    command.Parameters.AddWithValue("@orderId", item.OrderId);
                    command.Parameters.AddWithValue("@productId", item.ProductId);
                    command.Parameters.AddWithValue("@quantity", item.Quantity);
                    item.OrderItemId = Convert.ToInt32(command.ExecuteScalar());
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
        }
    }
}
