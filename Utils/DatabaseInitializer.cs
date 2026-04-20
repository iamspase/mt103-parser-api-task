using Microsoft.Data.Sqlite;
using NLog;

namespace SwiftMT103ApiTask.Utils
{
    public class DatabaseInitializer
    {
        private string _connectionString;

        public readonly Logger Logger = LogManager.GetCurrentClassLogger();


        public DatabaseInitializer(string connectionString)
        {
            this._connectionString = connectionString;
        }

        /// <summary>
        /// Creates the database tables such as MT103Messages if it does not exist
        /// </summary>
        /// <returns></returns>
        public async Task Initialize()
        {
           try
            {
                 using var connection = new SqliteConnection(this._connectionString);
                await connection.OpenAsync();

                var sql = File.ReadAllText("init.sql");

                using var command = connection.CreateCommand();
                command.CommandText = sql;
                Logger.Info("Initializing database and tables if they do not exists.");

                await command.ExecuteNonQueryAsync();
            }
            catch(Exception e)
            {
                Logger.Error(e, "Something went wrong while establishing a connection to the database.");
            }
        }
    }
}
