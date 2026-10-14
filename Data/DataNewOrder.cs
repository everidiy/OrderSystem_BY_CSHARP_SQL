using Microsoft.Data.Sqlite;
using Orders_System_c__sql.Models;
using OrdersSystemSql;
using System;

namespace Orders_System_c__sql.Data
{
    public class DataNewOrder
    {
        public int Create(int customerId)
        {
            using var connection =
                new SqliteConnection(Database.ConnectionString);

            connection.Open();

            string sql = """
                INSERT INTO Orders (CustomerId, CreatedAt, Status)
                VALUES (@customerId, @createdAt, @status);

                SELECT last_insert_rowid();
                """;

            using var command = new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue("@customerId", customerId);
            command.Parameters.AddWithValue("@createdAt", DateTime.Now);
            command.Parameters.AddWithValue("@status", "New");

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public void DeleteItem(int orderId, OrderItem item)
        {
            using var connection =
                new SqliteConnection(Database.ConnectionString);

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                string deleteSql = """
                    DELETE FROM OrderItems
                    WHERE OrderId = @orderId
                    AND ProductId = @productId;
                    """;

                using (var command = new SqliteCommand(
                    deleteSql, connection, transaction))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);
                    command.Parameters.AddWithValue("@productId", item.ProductId);
                    command.ExecuteNonQuery();
                }

                string updateStockSql = """
                    UPDATE Products
                    SET Stock = Stock + @quantity
                    WHERE ProductId = @productId;
                    """;

                using (var command = new SqliteCommand(
                    updateStockSql, connection, transaction))
                {
                    command.Parameters.AddWithValue("@quantity", item.Quantity);
                    command.Parameters.AddWithValue("@productId", item.ProductId);
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Save(int orderId, int customerId)
        {
            using var connection =
                new SqliteConnection(Database.ConnectionString);

            connection.Open();

            string sql = """
                UPDATE Orders
                SET CustomerId = @customerId
                WHERE OrderId = @orderId;
                """;

            using var command = new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue("@customerId", customerId);
            command.Parameters.AddWithValue("@orderId", orderId);

            command.ExecuteNonQuery();
        }
    }
}