using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Microsoft.Data.Sqlite;
using Grocery.Core.Data.Helpers;

namespace Grocery.Core.Data.Repositories
{
    public class GroceryListItemsRepository : DatabaseConnection ,IGroceryListItemsRepository
    {
        private readonly List<GroceryListItem> groceryListItems;

        public GroceryListItemsRepository()
        {
            CreateTable(@"CREATE TABLE IF NOT EXISTS GroceryListItems (
                            [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                            [GroceryListId] INTEGER NOT NULL,
                            [ProductId] INTEGER NOT NULL, 
                            [Amount] INTEGER NOT NULL)");
            List<string> insertQueries = [@"INSERT OR IGNORE INTO GroceryList(GroceryListId, ProductId, Amount) VALUES(1, 1, 3)",
                                          @"INSERT OR IGNORE INTO GroceryList(GroceryListId, ProductId, Amount) VALUES(1, 2, 1)",
                                          @"INSERT OR IGNORE INTO GroceryList(GroceryListId, ProductId, Amount) VALUES(1, 3, 4)",
                                          @"INSERT OR IGNORE INTO GroceryList(GroceryListId, ProductId, Amount) VALUES(2, 1, 2)",
                                          @"INSERT OR IGNORE INTO GroceryList(GroceryListId, ProductId, Amount) VALUES(2, 2, 5)"];
            groceryListItems = new();
            GetAll();
        }

        public List<GroceryListItem> GetAll()
        {
            if (groceryListItems != null) groceryListItems.Clear();

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

        public List<GroceryListItem> GetAllOnGroceryListId(int id)
        {
            return groceryListItems.Where(g => g.GroceryListId == id).ToList();
        }

        public List<GroceryListItem> GetAllOnGroceryItemId(int? productId)
        {
            List<GroceryListItem> groceryItems = new List<GroceryListItem>();

            foreach (GroceryListItem item in groceryListItems)
            {
                if (item.ProductId == productId)
                {
                    groceryItems.Add(item);
                }
            }

            return groceryItems;
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
                item.Id = Convert.ToInt32(command.ExecuteScalar());
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
            return groceryListItems.FirstOrDefault(g => g.Id == id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            GroceryListItem? listItem = groceryListItems.FirstOrDefault(i => i.Id == item.Id);
            listItem = item;
            return listItem;
        }
    }
}
