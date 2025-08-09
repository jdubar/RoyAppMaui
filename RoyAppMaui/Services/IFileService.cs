using RoyAppMaui.Models;

namespace RoyAppMaui.Services;
public interface IFileService
{
    IEnumerable<Sleep> ImportSleepDataFromCsv(string filePath);
    string GetExportData(IEnumerable<Sleep> sleeps);
    Task<bool> SaveDataToFile(string data);
    Task<FileResult?> SelectImportFile();
}
