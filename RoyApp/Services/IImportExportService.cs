namespace RoyApp.Services;
public interface IImportExportService
{
    event Action? OnExportRequested;
    event Action? OnImportRequested;
    void RequestExport();
    void RequestImport();
}
