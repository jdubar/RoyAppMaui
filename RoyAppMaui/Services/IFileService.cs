using FluentResults;

using RoyAppMaui.Models;

namespace RoyAppMaui.Services;
public interface IFileService
{
    Result<List<Sleep>> GetSleepDataFromCsv(string filePath);
    string GetExportData(IEnumerable<Sleep> sleeps);
    Task<bool> SaveDataToFile(string data);
    Task<FileResult?> SelectImportFile();
}
