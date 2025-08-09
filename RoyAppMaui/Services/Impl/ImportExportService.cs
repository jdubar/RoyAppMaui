namespace RoyAppMaui.Services.Impl;
[[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "No need to cover actions.")]
public class ImportExportService : IImportExportService
{
    public event Action? OnExportRequested;
    public event Action? OnImportRequested;
    public void RequestExport() => OnExportRequested?.Invoke();
    public void RequestImport() => OnImportRequested?.Invoke();
}
