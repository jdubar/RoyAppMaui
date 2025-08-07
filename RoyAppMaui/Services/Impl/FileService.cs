using CommunityToolkit.Maui.Storage;

using Microsoft.VisualBasic.FileIO;

using RoyAppMaui.Extensions;
using RoyAppMaui.Models;
using System.Collections.ObjectModel;
using System.Text;

namespace RoyAppMaui.Services.Impl;
public class FileService(IDataService dataService, IFileSaver fileSaver) : IFileService
{
    private static readonly string[] _ios = ["public.comma-separated-values-text"];
    private static readonly string[] _android = ["text/comma-separated-values"];
    private static readonly string[] _win = [".csv"];
    private static readonly string[] _tizen = ["*/*"];
    private static readonly string[] _mac = ["UTType.commaSeparatedText"];

    public ObservableCollection<Sleep> ParseImportFileData(string selectedFile)
    {
        var items = new ObservableCollection<Sleep>();
        using var parser = new TextFieldParser(selectedFile);
        parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
        parser.SetDelimiters(",");
        while (!parser.EndOfData)
        {
            var sleep = new Sleep();
            var fields = parser.ReadFields();
            if (fields != null && fields.Length == 3)
            {
                sleep.Id = fields[0];

                sleep.Bedtime = fields[1].ToTimeSpan();
                sleep.Waketime = fields[2].ToTimeSpan();

                sleep.Duration = dataService.GetDuration(sleep.BedtimeRec, sleep.WaketimeRec);

                items.Add(sleep);
            }
        }
        return items;
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
