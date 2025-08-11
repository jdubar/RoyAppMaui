using CommunityToolkit.Maui.Storage;

using CsvHelper;
using CsvHelper.Configuration;

using FluentResults;

using RoyAppMaui.ClassMaps;
using RoyAppMaui.Extensions;
using RoyAppMaui.Models;

using System.Globalization;
using System.Text;

namespace RoyAppMaui.Services.Impl;
public class FileService(System.IO.Abstractions.IFileSystem fileSystem, IFileSaver fileSaver, IFilePicker filePicker) : IFileService
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

    public async Task<bool> SaveDataToFile(IEnumerable<Sleep> sleeps)
    {
        if (!sleeps.Any())
        {
            return false;
        }

        var data = GetExportData(sleeps);
        using var stream = new MemoryStream(Encoding.Default.GetBytes(data));

        var now = DateTime.Now;
        var result = await fileSaver.SaveAsync($"RoyApp_{now:yyyy-MM-dd}_{now:hh:mm:ss}.csv", stream);
        return result.IsSuccessful;
    }

    public async Task<FileResult?> SelectImportFile()
    {
        PickOptions options = new()
        {
            PickerTitle = "Please select an import file",
            FileTypes = GetFilePickerFileTypes()
        };
        var file = await filePicker.PickAsync(options);
        return file;
    }

    private static string GetExportData(IEnumerable<Sleep> sleeps)
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

    private static FilePickerFileType? GetFilePickerFileTypes()
    {
        return new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, ["public.comma-separated-values-text"] },
                { DevicePlatform.Android, ["text/comma-separated-values"] },
                { DevicePlatform.WinUI, [".csv"] },
                { DevicePlatform.Tizen, ["*/*"] },
                { DevicePlatform.macOS, ["UTType.commaSeparatedText"] }
            });
    }
}
