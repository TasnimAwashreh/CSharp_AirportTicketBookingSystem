using Microsoft.Data.Sqlite;

namespace ATB.Data.Db
{
    public class DatabaseManager
    {
        private string _connString;

        public DatabaseManager(string connString)
        {
            _connString = connString;
        }
        public void CreateDatabase()
        {
            using (var conn = new SqliteConnection($"Data Source={_connString}"))
            {
                conn.Open();
                var createManagerTable = @"CREATE TABLE IF NOT EXISTS User (
                                            UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Name TEXT NOT NULL,
                                            Password TEXT NOT NULL,
                                            Type TEXT NOT NULL
                                        );

                                            CREATE TABLE IF NOT EXISTS Booking (
                                            BookingId INTEGER PRIMARY KEY AUTOINCREMENT,
                                            FlightId INTEGER NOT NULL,
                                            PassengerId INTEGER NOT NULL,
                                            BookingClass TEXT NOT NULL
                                        );";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = createManagerTable;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
