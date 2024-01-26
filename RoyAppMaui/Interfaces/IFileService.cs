using RoyAppMaui.Models;

using System.Collections.ObjectModel;

namespace RoyAppMaui.Interfaces;
public interface IFileService
{
    ObservableCollection<Sleep> ParseImportFileData(string selectedFile);
    Task<FileResult?> SelectImportFile();
}
