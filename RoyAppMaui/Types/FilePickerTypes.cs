namespace RoyAppMaui.Types;

/// <summary>
/// Provides file picker types for different device platforms.
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "No logic to test here")]
public static class FilePickerTypes
{
    private static readonly Dictionary<DevicePlatform, IEnumerable<string>> _fileTypes = new()
    {
        { DevicePlatform.iOS, ["public.comma-separated-values-text"] },
        { DevicePlatform.Android, ["text/comma-separated-values"] },
        { DevicePlatform.WinUI, [".csv"] },
        { DevicePlatform.Tizen, ["*/*"] },
        { DevicePlatform.macOS, ["UTType.commaSeparatedText"] }
    };

    /// <summary>
    /// Gets the file picker types for supported platforms (CSV files).
    /// </summary>
    /// <returns>A <see cref="FilePickerFileType"/> configured for CSV files on all supported platforms.</returns>
    public static FilePickerFileType GetFilePickerFileTypes() => new(_fileTypes);
}
