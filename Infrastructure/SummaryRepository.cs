using Microsoft.Data.Sqlite;

namespace DailyLearning.Infrastructure;

public class SummaryRepository
{
    /**
     * Local JSON File
     */
    private static readonly string JsonDataPath = Path.Combine(AppContext.BaseDirectory, "Data", "summaries.json");

    public string GetSummariesFromLocalJson()
    {
        return File.ReadAllText(JsonDataPath);
    }
    
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
            CREATE TABLE IF NOT EXISTS summaries (
                id INTEGER PRIMARY KEY,
                chapter TEXT NOT NULL,
                summary TEXT
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
        truncateTableCommand.CommandText = "DELETE FROM summaries; VACUUM;";
        truncateTableCommand.ExecuteNonQuery();
        connection.Close();
    }
    
    public string GetByChapterReference(string chapterReference)
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        var readCommand = connection.CreateCommand();
        readCommand.CommandText = "SELECT summary FROM summaries WHERE chapter = $chapter LIMIT 1";
        readCommand.Parameters.AddWithValue("$chapter", chapterReference);

        var summary = "";

        using var reader = readCommand.ExecuteReader();
        if (reader.Read())
        {
            summary = reader.GetString(0);
        }
        return summary;
    }

    public bool Insert(int index, string chapter, string summary)
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO summaries (id, chapter, summary)
            VALUES ($id, $chapter, $summary);
        """;
        command.Parameters.AddWithValue("$id", index);
        command.Parameters.AddWithValue("$chapter", chapter);
        command.Parameters.AddWithValue("$summary", summary);

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