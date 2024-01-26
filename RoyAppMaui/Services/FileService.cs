using Microsoft.VisualBasic.FileIO;

using RoyAppMaui.Interfaces;
using RoyAppMaui.Models;

using System.Collections.ObjectModel;

namespace RoyAppMaui.Services;
public class FileService(IDateTimeService dateTimeService) : IFileService
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
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        while (!parser.EndOfData)
        {
            var sleep = new Sleep();
            var fields = parser.ReadFields();
            if (fields != null && fields.Length == 3)
            {
                sleep.Id = fields[0];

                sleep.Bedtime = dateTimeService.StringToTimeSpan(fields[1]);
                sleep.BedtimeRec = dateTimeService.TimeSpanToDecimal(sleep.Bedtime);
                sleep.BedtimeDisplay = dateTimeService.TimeSpanToDateTime((TimeSpan)sleep.Bedtime);

                sleep.Waketime = dateTimeService.StringToTimeSpan(fields[2]);
                sleep.WaketimeRec = dateTimeService.TimeSpanToDecimal(sleep.Waketime);
                sleep.WaketimeDisplay = dateTimeService.TimeSpanToDateTime((TimeSpan)sleep.Waketime);

                sleep.Duration = dateTimeService.GetDuration(sleep.BedtimeRec, sleep.WaketimeRec);

                items.Add(sleep);
            }
        }
        return items;
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
