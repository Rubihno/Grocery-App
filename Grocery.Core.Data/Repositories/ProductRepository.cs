using Grocery.Core.Data.Helpers;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Microsoft.Data.Sqlite;

namespace Grocery.Core.Data.Repositories
{
    public class ProductRepository : DatabaseConnection ,IProductRepository
    {
        private readonly List<Product> products = [];
        public ProductRepository()
        {
            CreateTable(@"CREATE TABLE IF NOT EXISTS Products (
                    [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    [Name] NVARCHAT(80) UNIQUE NOT NULL,
                    [Stock] INTEGER NOT NULL, 
                    [Price] DECIMAL(10, 2) NOT NULL, 
                    [Date] DATE NOT NULL)");
            List<string> insertQueries = [@"INSERT OR IGNORE INTO Products(Name, Stock, Price, Date) Values('Melk', 300, 1.45, '2025-11-25')",
                                          @"INSERT OR IGNORE INTO Products(Name, Stock, Price, Date) Values('Kaas', 100, 4.50, '2025-11-30')",
                                          @"INSERT OR IGNORE INTO Products(Name, Stock, Price, Date) Values('Brood', 400, 1.99, '2025-11-12')",
                                          @"INSERT OR IGNORE INTO Products(Name, Stock, Price, Date) Values('Cornflakes', 0, 2.99, '2026-02-28')"];
            InsertMultipleWithTransaction(insertQueries);
            GetAll();
        }
        public List<Product> GetAll()
        {
            try
            {
                products.Clear();

                string selectQuery = "SELECT * FROM Products";
                OpenConnection();

                using (SqliteCommand command = new(selectQuery, Connection))
                {
                    SqliteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        int stock = reader.GetInt32(2);
                        decimal price = reader.GetDecimal(3);
                        DateOnly date = DateOnly.FromDateTime(reader.GetDateTime(4));

                        products.Add(new(id, name, stock, price, date));
                    }
                    return products;
                }
            }
            catch (SqliteException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sqlite error while getting all products: {ex.Message}");
                return new List<Product>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error while getting all products: {ex.Message}");
                return new List<Product>();
            }
            finally
            {
                CloseConnection();
            }
        }

        public Product? Get(int? productId)
        {
            try
            {
                string selectQuery = $"SELECT Id, Name, Stock, Price, date(Date) FROM Products WHERE Id = {productId.Value}";
                Product? p = null;
                OpenConnection();

                using (SqliteCommand command = new(selectQuery, Connection))
                {
                    SqliteDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        int stock = reader.GetInt32(2);
                        decimal price = reader.GetDecimal(3);
                        DateOnly date = DateOnly.FromDateTime(reader.GetDateTime(4));

                        p = new(id, name, stock, price, date);
                    }
                }
                return p;
            }
            catch (SqliteException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sqlite error while getting product: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error while getting product: {ex.Message}");
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public Product? Add(Product? item)
        {
            try
            {
                int recordsAffected;
                string insertQuery = $"INSERT INTO Products(Name, Stock, Price, Date) VALUES(@Name, @Stock, @Price, @Date) Returning RowId";
                OpenConnection();

                using (SqliteCommand command = new(insertQuery, Connection))
                {
                    command.Parameters.AddWithValue("Name", item.Name);
                    command.Parameters.AddWithValue("Stock", item.Stock);
                    command.Parameters.AddWithValue("Price", item.Price);
                    command.Parameters.AddWithValue("Date", item.ShelfLife);

                    // recordsAffected = command.ExecuteNonQuery();
                    item.Id = Convert.ToInt32(command.ExecuteScalar());
                }
                return item;
            }
            catch (SqliteException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sqlite error while adding product: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error while adding product: {ex.Message}");
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public Product? Delete(Product item)
        {
            try
            {
                string deleteQuery = $"DELETE FROM Products WHERE Id = {item.Id}";
                OpenConnection();
                Connection.ExecuteNonQuery(deleteQuery);
                return item;
            }
            catch (SqliteException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sqlite error while deleting product: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error while deleting product: {ex.Message}");
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public Product? Update(Product item)
        {
            try
            {
                int recordsAffected;
                string updateQuery = $"UPDATE Products SET Name = @Name, Stock = @Stock, Price = @Price, Date = @Date WHERE Id = {item.Id}";
                OpenConnection();

                using (SqliteCommand command = new(updateQuery, Connection))
                {
                    command.Parameters.AddWithValue("Name", item.Name);
                    command.Parameters.AddWithValue("Stock", item.Stock);
                    command.Parameters.AddWithValue("Price", item.Price);
                    command.Parameters.AddWithValue("Date", item.ShelfLife);

                    recordsAffected = command.ExecuteNonQuery();
                }
                return item;
            }
            catch (SqliteException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sqlite error while updating product: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error while updating product: {ex.Message}");
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
