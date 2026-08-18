using Microsoft.Data.Sqlite;

namespace DailyLearning.Infrastructure;

public class PropertiesRepository
{
    /**
     * SQLite Database
     */
    private static readonly string DatabasePath = Path.Combine(AppContext.BaseDirectory, "Data", "summaries.sqlite");
    private static readonly string ConnectionString = $"Data Source={DatabasePath}";

    public void CreateTableIfNotExists()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var createTableCmd = connection.CreateCommand();
        createTableCmd.CommandText = """
                                         CREATE TABLE IF NOT EXISTS properties (
                                             key TEXT NOT NULL,
                                             value TEXT NOT NULL
                                         );
                                     """;
        createTableCmd.ExecuteNonQuery();
        connection.Close();
    }

    public void TruncateTable()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var truncateTableCommand = connection.CreateCommand();
        truncateTableCommand.CommandText = "DELETE FROM properties; VACUUM;";
        truncateTableCommand.ExecuteNonQuery();
        connection.Close();
    }
    
    public string GetValue(string key)
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var readCommand = connection.CreateCommand();
        readCommand.CommandText = "SELECT value FROM properties WHERE key = $key LIMIT 1";
        readCommand.Parameters.AddWithValue("key", key);

        var result = "";

        using var reader = readCommand.ExecuteReader();
        if (reader.Read())
        {
            result = reader.GetString(0);
        }
        return result;
    }

    public bool Insert(string key, string value)
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = """
                                  INSERT INTO properties (key, value)
                                  VALUES ($key, $value);
                              """;
        command.Parameters.AddWithValue("$key", key);
        command.Parameters.AddWithValue("$value", value);

        try
        {
            var rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
        finally
        {
            connection.Close();
        }
    }
}