using CsvHelper;
using CsvHelper.Configuration;

using FluentResults;

using RoyAppMaui.ClassMaps;
using RoyAppMaui.Extensions;
using RoyAppMaui.Models;

using System.Globalization;
using System.Text;

namespace RoyAppMaui.Services.Impl;
public class FileService(System.IO.Abstractions.IFileSystem fileSystem) : IFileService
{
    public Result<List<Sleep>> GetSleepDataFromCsv(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return Result.Fail("File path is null or empty.");
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false
        };

        try
        {
            using var reader = fileSystem.File.OpenText(filePath);
            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<SleepMap>();
            var records = csv.GetRecords<Sleep>().ToList();

            return records.Count > 0
                ? Result.Ok(records)
                : Result.Fail("No sleep records found in the CSV file.");
        }
        catch (FileNotFoundException)
        {
            return Result.Fail($"The file at {filePath} was not found.");
        }
        catch (CsvHelperException ex)
        {
            return Result.Fail($"CSV parsing error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Fail($"An unexpected error occurred while reading the file: {ex.Message}");
        }
    }

    public string GetExportData(IEnumerable<Sleep> sleeps)
    {
        const string header = "Id,Bedtime,Bedtime (as decimal),Waketime,Waketime (as decimal),Duration";

        var sb = new StringBuilder();
        sb.AppendLine(header);
        foreach (var sleep in sleeps)
        {
            sb.AppendLine($"{sleep.Id},{sleep.BedtimeDisplay},{sleep.BedtimeRec},{sleep.WaketimeDisplay},{sleep.WaketimeRec},{sleep.Duration}");
        }

        sb.AppendLine($"Bedtime Average: {sleeps.GetAverage(s => s.BedtimeRec)}");
        sb.AppendLine($"Waketime Average: {sleeps.GetAverage(s => s.WaketimeRec)}");
        sb.AppendLine($"Duration Average: {sleeps.GetAverage(s => s.Duration)}");
        return sb.ToString();
    }
}
