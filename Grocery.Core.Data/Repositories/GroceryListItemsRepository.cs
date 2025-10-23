using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Microsoft.Data.Sqlite;
using Grocery.Core.Data.Helpers;

namespace Grocery.Core.Data.Repositories
{
    public class GroceryListItemsRepository : DatabaseConnection ,IGroceryListItemsRepository
    {
        private readonly List<GroceryListItem> groceryListItems = [];
        public List<GroceryListItem> data = [                
            new GroceryListItem(1, 1, 1, 3),
            new GroceryListItem(2, 1, 2, 1),
            new GroceryListItem(3, 1, 3, 4),
            new GroceryListItem(4, 2, 1, 2),
            new GroceryListItem(5, 2, 2, 5),
        ];

        public GroceryListItemsRepository()
        {
            CreateTable(@"CREATE TABLE IF NOT EXISTS GroceryListItems (
                            [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                            [GroceryListId] INTEGER NOT NULL,
                            [ProductId] INTEGER NOT NULL,
                            [Amount] INTEGER NOT NULL,
                            Unique(GroceryListId, ProductId)
                        )");

            List<string> insertQueries = [
                @"INSERT OR REPLACE INTO GroceryListItems(GroceryListId, ProductId, Amount) VALUES(1, 1, 3)",
                @"INSERT OR REPLACE INTO GroceryListItems(GroceryListId, ProductId, Amount) VALUES(1, 2, 1)",
                @"INSERT OR REPLACE INTO GroceryListItems(GroceryListId, ProductId, Amount) VALUES(1, 3, 4)",
                @"INSERT OR REPLACE INTO GroceryListItems(GroceryListId, ProductId, Amount) VALUES(2, 1, 2)",
                @"INSERT OR REPLACE INTO GroceryListItems(GroceryListId, ProductId, Amount) VALUES(2, 2, 5)"
            ];

            InsertMultipleWithTransaction(insertQueries);
            GetAll();
        }

        public List<GroceryListItem> GetAll()
        {
            groceryListItems.Clear();

            string selectQuery = "SELECT * FROM GroceryListItems";
            OpenConnection();

            using (SqliteCommand command = new(selectQuery, Connection))
            {
                SqliteDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int groceryListId = reader.GetInt32(1);
                    int productId = reader.GetInt32(2);
                    int amount = reader.GetInt32(3);

                    groceryListItems.Add(new(id, groceryListId, productId, amount));
                }
            }
            CloseConnection();
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryItems = [];
            string selectQuery = $"SELECT Id, ProductId, Amount FROM GroceryListItems WHERE GroceryListId = {groceryListId}";
            OpenConnection();

            using (SqliteCommand command = new(selectQuery, Connection))
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int productId = reader.GetInt32(1);
                    int amount = reader.GetInt32(2);
                    groceryItems.Add(new(id, groceryListId, productId, amount));
                }
            }

            CloseConnection();
            return groceryItems;
        }

        public List<GroceryListItem> GetAllOnGroceryItemId(int? productId)
        {
            List<GroceryListItem> groceryListItems = [];
            string selectQuery = $"SELECT Id, GroceryListId, Amount FROM GroceryListItems WHERE ProductId = {productId.Value}";
            OpenConnection();

            using (SqliteCommand command = new(selectQuery, Connection))
            {
                
                SqliteDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    int groceryListId = reader.GetInt32(1);
                    int amount = reader.GetInt32(2);

                    groceryListItems.Add(new(id, groceryListId, productId.Value, amount));
                }
            }
            CloseConnection();
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            int recordsAffected;
            string insertQuery = $"INSERT INTO GroceryListItems(GroceryListId, ProductId, Amount) VALUES(@GroceryListId, @ProductId, @Amount) Returning RowId;";
            OpenConnection();

            using (SqliteCommand command = new(insertQuery, Connection))
            {
                command.Parameters.AddWithValue("GroceryListId", item.GroceryListId);
                command.Parameters.AddWithValue("ProductId", item.ProductId);
                command.Parameters.AddWithValue("Amount", item.Amount);

                //recordsAffected = command.ExecuteNonQuery();
                item.Id = Convert.ToInt32(command.ExecuteScalar());
            }
            CloseConnection();
            return item;
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            try
            {
                string deleteQuery = $"DELETE FROM GroceryListItems WHERE Id = {item.Id}";
                OpenConnection();
                Connection.ExecuteNonQuery(deleteQuery);
                return item;
            }
            catch (SqliteException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sqlite error while deleting GroceryListItem: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error while deleting GroceryListItem: {ex.Message}");
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public GroceryListItem? Get(int id)
        {
            try
            {
                string selectQuery = $"SELECT Id, GroceryListId, ProductId, Amount FROM GroceryListItems WHERE Id = {id}";
                GroceryListItem? groceryListItem = null;
                OpenConnection();

                using (SqliteCommand command = new(selectQuery, Connection))
                {
                    SqliteDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int Id = reader.GetInt32(0);
                        int groceryListId = reader.GetInt32(1);
                        int productId = reader.GetInt32(2);
                        int amount = reader.GetInt32(3);

                        groceryListItem = new(Id, groceryListId, productId, amount);
                    }
                }
                return groceryListItem;
            }
            catch (SqliteException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sqlite error while getting GroceryListItem: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error while getting GroceryListItem: {ex.Message}");
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            try
            {
                int recordsAffected;
                string updateQuery = $"UPDATE GroceryListItems SET GroceryListId = @GroceryListId, ProductId = @ProductId, Amount = @Amount WHERE Id = {item.Id}";
                OpenConnection();

                using (SqliteCommand command = new(updateQuery, Connection))
                {
                    command.Parameters.AddWithValue("GroceryListId", item.GroceryListId);
                    command.Parameters.AddWithValue("ProductId", item.ProductId);
                    command.Parameters.AddWithValue("Amount", item.Amount);

                    recordsAffected = command.ExecuteNonQuery();
                }
                return item;
            }
            catch (SqliteException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Sqlite error while updating GroceryListItem: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error while updating GroceryListItem: {ex.Message}");
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
