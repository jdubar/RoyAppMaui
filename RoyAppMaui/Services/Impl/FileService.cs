using CommunityToolkit.Maui.Storage;

using CsvHelper;
using CsvHelper.Configuration;

using RoyAppMaui.ClassMaps;
using RoyAppMaui.Extensions;
using RoyAppMaui.Models;

using System.Globalization;
using System.Text;

namespace RoyAppMaui.Services.Impl;
public class FileService(IFileSaver fileSaver) : IFileService
{
    private static readonly string[] _ios = ["public.comma-separated-values-text"];
    private static readonly string[] _android = ["text/comma-separated-values"];
    private static readonly string[] _win = [".csv"];
    private static readonly string[] _tizen = ["*/*"];
    private static readonly string[] _mac = ["UTType.commaSeparatedText"];

    public IEnumerable<Sleep> ImportSleepDataFromCsv(string filePath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<SleepMap>();
        var records = csv.GetRecords<Sleep>();
        return records.ToList();
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
        return await FilePicker.Default.PickAsync(options);
    }

    private static FilePickerFileType? GetFilePickerFileTypes()
    {
        return new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, _ios },
                { DevicePlatform.Android, _android },
                { DevicePlatform.WinUI, _win },
                { DevicePlatform.Tizen, _tizen },
                { DevicePlatform.macOS, _mac }
            });
    }
}
