using RoyAppMaui.Models;
using System.Collections.ObjectModel;

namespace RoyAppMaui.Services;
public interface IFileService
{
    event Action OnExportRequested;
    void RequestExport();
    string GetExportData(IEnumerable<Sleep> sleeps);
    ObservableCollection<Sleep> ParseImportFileData(string selectedFile);
    Task<bool> SaveDataToFile(string data);
    Task<FileResult?> SelectImportFile();
}
