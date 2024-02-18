using RoyAppMaui.Models;
using System.Collections.ObjectModel;

namespace RoyAppMaui.Services;
public interface IFileService
{
    ObservableCollection<Sleep> ParseImportFileData(string selectedFile);
    Task<bool> SaveDataToFile(string data);
    Task<FileResult?> SelectImportFile();
}
