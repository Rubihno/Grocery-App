using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Microsoft.Data.Sqlite;
using Grocery.Core.Data.Helpers;
using System.Diagnostics;

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
                            [Amount] INTEGER NOT NULL)");
            List<string> insertQueries = [@"INSERT OR IGNORE INTO GroceryListItems(GroceryListId, ProductId, Amount) VALUES(1, 1, 3)",
                                          @"INSERT OR IGNORE INTO GroceryListItems(GroceryListId, ProductId, Amount) VALUES(1, 2, 1)",
                                          @"INSERT OR IGNORE INTO GroceryListItems(GroceryListId, ProductId, Amount) VALUES(1, 3, 4)",
                                          @"INSERT OR IGNORE INTO GroceryListItems(GroceryListId, ProductId, Amount) VALUES(2, 1, 2)",
                                          @"INSERT OR IGNORE INTO GroceryListItems(GroceryListId, ProductId, Amount) VALUES(2, 2, 5)"];
            
            /* 
               Zonder RowInDb uit te voeren wordt bij elke start van de app 
               elke GroceryListItem in insertQueries opnieuw toegevoegd.
            */
            bool rowInDb = RowInDb();
            if (!rowInDb) InsertMultipleWithTransaction(insertQueries);
            GetAll();
        }

        public bool RowInDb()
        {
            bool rowInDb = true;
            foreach (GroceryListItem d in data)
            {
                string query = "SELECT EXISTS(SELECT 1 FROM GroceryListItems WHERE GroceryListId = @GroceryListId AND ProductId = @ProductId)";
                using (var command = new SqliteCommand(query, Connection))
                {
                    command.Parameters.AddWithValue("@GroceryListId", d.GroceryListId);
                    command.Parameters.AddWithValue("@ProductId", d.ProductId);
                    var result = command.ExecuteScalar();
                    string existsAsString = result.ToString();

                    if (existsAsString == "0") rowInDb = false;
                }
            }

            return rowInDb;
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
            Debug.WriteLine(productId);
            List<GroceryListItem> gli = [];
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

                    gli.Add(new(id, groceryListId, productId.Value, amount));
                }
            }
            CloseConnection();
            return gli;
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

                recordsAffected = command.ExecuteNonQuery();
            }
            CloseConnection();
            return item;
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            string deleteQuery = $"DELETE FROM GroceryListItems WHERE Id = {item.Id}";
            OpenConnection();
            Connection.ExecuteNonQuery(deleteQuery);
            CloseConnection();
            return item;
        }

        public GroceryListItem? Get(int id)
        {
            string selectQuery = $"SELECT Id, GroceryListId, ProductId, Amount FROM GroceryListItems WHERE Id = {id}";
            GroceryListItem? gli = null;
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

                    gli = new(Id, groceryListId, productId, amount);
                }
            }

            CloseConnection();
            return gli;
        }

        public GroceryListItem? Update(GroceryListItem item)
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

            CloseConnection();
            return item;
        }
    }
}
