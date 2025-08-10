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
        catch (DirectoryNotFoundException)
        {
            return Result.Fail($"The directory for the file at {filePath} was not found.");
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
        var sb = new StringBuilder();
        _ = sb.AppendLine("Id,Bedtime,Bedtime (as decimal),Waketime,Waketime (as decimal)");
        foreach (var sleep in sleeps)
        {
            _ = sb.AppendLine($"{sleep.Id},{sleep.BedtimeDisplay},{sleep.BedtimeRec},{sleep.WaketimeDisplay},{sleep.WaketimeRec}");
        }
        _ = sb.AppendLine($"\r\nBedtime Average: {sleeps.GetAverage(s => s.BedtimeRec)}");
        _ = sb.AppendLine($"Waketime Average: {sleeps.GetAverage(s => s.WaketimeRec)}");
        return sb.ToString();
    }

    public async Task<bool> SaveDataToFile(string data)
    {
        using var stream = new MemoryStream(Encoding.Default.GetBytes(data));
        var result = await fileSaver.SaveAsync($"RoyApp_{DateTime.Now:yyyy-MM-dd}_{DateTime.Now:hh:mm:ss}.csv", stream);
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
