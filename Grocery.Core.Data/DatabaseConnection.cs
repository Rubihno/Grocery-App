
using Grocery.Core.Data.Helpers;
using Microsoft.Data.Sqlite;

namespace Grocery.Core.Data
{
    public abstract class DatabaseConnection : IDisposable
    {
        protected SqliteConnection Connection { get; }
        string databaseName;

        public DatabaseConnection()
        {
            databaseName = ConnectionHelper.ConnectionStringValue("GroceryAppDb");
            //string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dbpath = "Data Source="+ Path.Combine(projectDirectory + databaseName);
            Connection = new SqliteConnection(dbpath);
        }

        protected void OpenConnection()
        {
            if (Connection.State != System.Data.ConnectionState.Open) Connection.Open();
        }

        protected void CloseConnection()
        {
            if (Connection.State != System.Data.ConnectionState.Closed) Connection.Close();
        }

        public void CreateTable(string commandText)
        {
            OpenConnection();
            using (var command = Connection.CreateCommand())
            {
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
        }

        public void InsertMultipleWithTransaction(List<string> linesToInsert)
        {
            OpenConnection();
            var transaction = Connection.BeginTransaction();

            try
            {
                linesToInsert.ForEach(l => Connection.ExecuteNonQuery(l));
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                transaction.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
