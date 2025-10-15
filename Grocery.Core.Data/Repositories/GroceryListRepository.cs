using Grocery.Core.Data.Helpers;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Microsoft.Data.Sqlite;

namespace Grocery.Core.Data.Repositories
{
    public class GroceryListRepository : DatabaseConnection, IGroceryListRepository
    {
        private readonly List<GroceryList> groceryLists = [];

        public GroceryListRepository()
        { 
            //ISO 8601 format: date.ToString("o", CultureInfo.InvariantCulture)
            CreateTable(@"CREATE TABLE IF NOT EXISTS GroceryList (
                            [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                            [Name] NVARCHAR(80) UNIQUE NOT NULL,
                            [Date] DATE NOT NULL,
                            [Color] NVARCHAR(12) NOT NULL,
                            [ClientId] INTEGER NOT NULL)");
            List<string> insertQueries = [@"INSERT OR IGNORE INTO GroceryList(Name, Date, Color, ClientId) VALUES('Boodschappen familieweekend', '2024-12-14', '#FF6A00', 1)",
                                          @"INSERT OR IGNORE INTO GroceryList(Name, Date, Color, ClientId) VALUES('Kerstboodschappen', '2024-12-07', '#626262', 1)",
                                          @"INSERT OR IGNORE INTO GroceryList(Name, Date, Color, ClientId) VALUES('Weekend boodschappen', '2024-11-30', '#003300', 1)"];
            InsertMultipleWithTransaction(insertQueries);
            GetAll();
        }

        public List<GroceryList> GetAll()
        {
            groceryLists.Clear();
            
            string selectQuery = "SELECT Id, Name, date(Date), Color, ClientId FROM GroceryList";
            OpenConnection();
            using (SqliteCommand command = new(selectQuery, Connection))
            {
                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    DateOnly date = DateOnly.FromDateTime(reader.GetDateTime(2));
                    string color = reader.GetString(3);
                    int clientId = reader.GetInt32(4);
                    groceryLists.Add(new(id, name, date, color, clientId));
                }
            }
            CloseConnection();
            return groceryLists;
        }
        public GroceryList Add(GroceryList item)
        {
            int recordsAffected;
            string insertQuery = $"INSERT INTO GroceryList(Name, Date, Color, ClientId) VALUES(@Name, @Date, @Color, @ClientId) Returning RowId;";
            OpenConnection();
            using (SqliteCommand command = new(insertQuery, Connection))
            {
                command.Parameters.AddWithValue("Name", item.Name);
                command.Parameters.AddWithValue("Date", item.Date);
                command.Parameters.AddWithValue("Color", item.Color);
                command.Parameters.AddWithValue("ClientId", item.ClientId);

                //recordsAffected = command.ExecuteNonQuery();
                item.Id = Convert.ToInt32(command.ExecuteScalar());
            }
            CloseConnection();
            return item;
        }

        public GroceryList? Delete(GroceryList item)
        {
            string deleteQuery = $"DELETE FROM GroceryList WHERE Id = {item.Id};";
            OpenConnection();
            Connection.ExecuteNonQuery(deleteQuery);
            CloseConnection();
            return item;
        }

        public GroceryList? Get(int id)
        {
            string selectQuery = $"SELECT Id, Name, date(Date), Color, ClientId FROM GroceryList WHERE Id = {id}";
            GroceryList? gl = null;
            OpenConnection();
            using (SqliteCommand command = new(selectQuery, Connection))
            {
                SqliteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int Id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    DateOnly date = DateOnly.FromDateTime(reader.GetDateTime(2));
                    string color = reader.GetString(3);
                    int clientId = reader.GetInt32(4);
                    gl = (new(Id, name, date, color, clientId));
                }
            }
            CloseConnection();
            return gl;
        }

        public GroceryList? Update(GroceryList item)
        {
            int recordsAffected;
            string updateQuery = $"UPDATE GroceryList SET Name = @Name, Date = @Date, Color = @Color  WHERE Id = {item.Id};";
            OpenConnection();
            using (SqliteCommand command = new(updateQuery, Connection))
            {
                command.Parameters.AddWithValue("Name", item.Name);
                command.Parameters.AddWithValue("Date", item.Date);
                command.Parameters.AddWithValue("Color", item.Color);

                recordsAffected = command.ExecuteNonQuery();
            }
            CloseConnection();
            return item;
        }
    }
}
