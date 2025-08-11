using CsvHelper;
using CsvHelper.Configuration;

using FluentResults;

using RoyAppMaui.ClassMaps;
using RoyAppMaui.Extensions;
using RoyAppMaui.Models;

using System.Globalization;

namespace RoyAppMaui.Services.Impl;
public class FileService(System.IO.Abstractions.IFileSystem fileSystem) : IFileService
{
    public Result<List<Sleep>> GetSleepDataFromCsv(string filePath)
    {
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

            return records.Count != 0
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
        var csvsleep = sleeps.Select(s =>
        {
            return $"{s.Id},{s.BedtimeDisplay},{s.BedtimeRec},{s.WaketimeDisplay},{s.WaketimeRec},{s.Duration}";
        });
        var data = string.Join(Environment.NewLine, csvsleep);
        var footer = $"{Environment.NewLine}Bedtime Average: {sleeps.GetAverage(s => s.BedtimeRec)}" +
                     $"{Environment.NewLine}Waketime Average: {sleeps.GetAverage(s => s.WaketimeRec)}" +
                     $"{Environment.NewLine}Duration Average: {sleeps.GetAverage(s => s.Duration)}";

        return $"{header}{Environment.NewLine}{data}{Environment.NewLine}{footer}";
    }
}
