using System.Text.Json;
using DailyLearning.Infrastructure;
using DailyLearning.Libraries;
using Microsoft.Data.Sqlite;

namespace DailyLearning.Seed;

public class FetchSummaries
{
    private const string VolumeId = "oldtestament";
    private readonly Sefaria _sefaria = new Sefaria();
    private readonly OpenScripture _openScripture = new OpenScripture();
    private readonly PropertiesRepository _propertiesRepository = new PropertiesRepository();
    private readonly SummaryRepository _summaryRepository = new SummaryRepository();

    private void InitializeAsync()
    {
        
    }

    private void CacheSefariaStartingDate()
    {
        
    }

    private async Task<bool> FetchChapterSummaries()
    {
        // Create Data folder if it doesn't exist
        var buildPath = AppContext.BaseDirectory;
        Directory.CreateDirectory(Path.Combine(buildPath, "Data"));
        
        _summaryRepository.CreateTableIfNotExists();
        _summaryRepository.TruncateTable();

        // Load all books from volume
        var books = await _openScripture.GetAllBooksByVolumeId(VolumeId);
        TerminalOutput.WriteLine($"{books.Count} books acquired. Fetching individual chapters.");
        
        var chapterDatabaseOrderIndex = 1;
        foreach (var book in books)
        {
            if (book.Chapters == null)
            {
                await book.Load();
                TerminalOutput.WriteBoldLine($"Book {book.Title}");

                if (book.Chapters == null) throw new Exception("No Chapters");

                for (var i = 0; i < book.Chapters.Count; i++)
                {
                    var chapter = book.Chapters[i];
                    
                    await chapter.Load();

                    if (chapter.Data?.Summary == null) throw new Exception("No chapter loaded");

                    var chapterTitle = $"{book.FormattedTitle} {chapter.Data.Number}";
                    var chapterSummary = chapter.Data.Summary;
                    
                    if (i+1 == book.Chapters.Count)
                    {
                        TerminalOutput.RewriteLine("");
                        TerminalOutput.WriteSuccessLine($"{i+1}/{book.Chapters.Count}\tLoaded {book.FormattedTitle} chapters.");
                    }
                    else
                    {
                        TerminalOutput.RewriteLine($"{i+1}/{book.Chapters.Count}\tLoading {chapterTitle}");
                    }

                    try
                    {
                        _summaryRepository.Insert(chapterDatabaseOrderIndex, chapterTitle, chapterSummary);
                        chapterDatabaseOrderIndex++;
                    } catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine($"Failed to insert chapter {chapterTitle} into database.");
                    }
                }
            }
        }

        return false;
    }

    public async Task<bool> GetChapterSummaries(bool fetchFromApi = false)
    {
        if (fetchFromApi)
        {
            return await FetchChapterSummaries();
        }

        var jsonData = _summaryRepository.GetSummariesFromLocalJson();
        var summaries = JsonSerializer.Deserialize<List<ChapterSummary>>(jsonData) ?? throw new Exception("Failed to deserialize ChapterSummary");

        _summaryRepository.CreateTableIfNotExists();
        _summaryRepository.TruncateTable();

        var chapterDatabaseOrderIndex = 1;
        foreach (var summary in summaries)
        {
            Console.WriteLine($"Importing {summary.Chapter}...");

            try
            {
                _summaryRepository.Insert(chapterDatabaseOrderIndex, summary.Chapter, summary.Summary ?? string.Empty);
                chapterDatabaseOrderIndex++;
                Console.WriteLine($"Successfully imported {summary.Chapter}.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine($"Failed to import {summary.Chapter}.");
            }
        }

        return false;
    }
}