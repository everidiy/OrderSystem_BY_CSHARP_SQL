using Microsoft.Data.Sqlite;

namespace OrdersSystemSql
{
    public static class Database
    {
        internal const string ConnectionString = "Data Source=customersNorders.db";

        public static void Initialize()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            string sql = """
            CREATE TABLE IF NOT EXISTS Customers (
                CustomerId INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Email TEXT NOT NULL
                    CHECK (
                        Email LIKE '%@%'
                        AND Email LIKE '%.%'
                    ),
                Phone TEXT NOT NULL
                    CHECK (
                        length(Phone) = 11
                        AND Phone NOT GLOB '*[^0-9]*'
                    )
            );

            CREATE TABLE IF NOT EXISTS Products
            (
                ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Price NUMERIC NOT NULL CHECK (Price >= 0),
                Stock INTEGER NOT NULL CHECK (Stock >= 0)
            );

            CREATE TABLE IF NOT EXISTS Orders
            (
                OrderId INTEGER PRIMARY KEY AUTOINCREMENT,
                CustomerId INTEGER NOT NULL,
                CreatedAt Datetime NOT NULL,
                Status TEXT CHECK (Status IN ('New', 'In progress', 'Completed')),

                FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId) 
                    ON UPDATE CASCADE 
                    ON DELETE CASCADE
            );

            CREATE TABLE IF NOT EXISTS OrderItems 
            (
                OrderItemId INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderId INTEGER NOT NULL,
                ProductId INTEGER NOT NULL,
                Quantity INTEGER NOT NULL CHECK (Quantity > 0),

                FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) 
                    ON UPDATE CASCADE 
                    ON DELETE CASCADE,
                FOREIGN KEY (ProductId) REFERENCES Products(ProductId) 
                    ON UPDATE CASCADE 
                    ON DELETE CASCADE
            );
            """;

            using var command = new SqliteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
}
