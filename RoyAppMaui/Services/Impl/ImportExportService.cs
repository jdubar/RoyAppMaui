namespace RoyAppMaui.Services.Impl;
public class ImportExportService : IImportExportService
{
    public event Action? OnExportRequested;
    public event Action? OnImportRequested;
    public void RequestExport() => OnExportRequested?.Invoke();
    public void RequestImport() => OnImportRequested?.Invoke();
}
