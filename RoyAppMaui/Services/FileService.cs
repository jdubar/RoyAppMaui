using RoyAppMaui.Interfaces;

namespace RoyAppMaui.Services;
public class FileService : IFileService
{
    private static readonly string[] _ios = ["public.comma-separated-values-text"];
    private static readonly string[] _android = ["text/comma-separated-values"];
    private static readonly string[] _win = [".csv"];
    private static readonly string[] _tizen = ["*/*"];
    private static readonly string[] _mac = ["UTType.commaSeparatedText"];

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
