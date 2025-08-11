namespace RoyAppMaui.Types;
public static class FilePickerTypes
{
    public static FilePickerFileType? GetFilePickerFileTypes()
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
