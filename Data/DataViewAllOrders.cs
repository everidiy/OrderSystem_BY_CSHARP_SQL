using Microsoft.Data.Sqlite;
using Orders_System_c__sql.Models;
using OrdersSystemSql;
using System;
using System.Collections.Generic;

namespace Orders_System_c__sql.Data
{
    public class DataViewAllOrders
    {
        public List<OrderWithDetails> GetAllRows()
        {
            List<OrderWithDetails> orders = new List<OrderWithDetails>();

            using (var connection =
                new SqliteConnection(Database.ConnectionString))
            {
                connection.Open();

                string sqlCommand = """
                    SELECT
                        o.OrderId,
                        o.CustomerId,
                        o.CreatedAt,
                        o.Status,

                        IFNULL(
                            group_concat(
                                p.Name || ' (x' || oi.Quantity || ')',
                                ', '
                            ),
                            'Null order'
                        ) AS ProductsList,

                        IFNULL(
                            SUM(p.Price * oi.Quantity),
                            0
                        ) AS TotalPrice

                    FROM Orders o
                    LEFT JOIN OrderItems oi
                        ON o.OrderId = oi.OrderId
                    LEFT JOIN Products p
                        ON oi.ProductId = p.ProductId

                    GROUP BY
                        o.OrderId,
                        o.CustomerId,
                        o.CreatedAt,
                        o.Status;
                    """;

                using (var command =
                    new SqliteCommand(sqlCommand, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderWithDetails order = new OrderWithDetails()
                            {
                                OrderId = reader.GetInt32(0),
                                CustomerId = reader.GetInt32(1),
                                CreatedAt = reader.GetDateTime(2),
                                Status = reader.GetString(3),
                                ProductsList = reader.GetString(4),
                                TotalPrice = reader.GetDecimal(5)
                            };

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }
    }
}