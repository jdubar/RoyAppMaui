using FluentResults;

using RoyAppMaui.Models;

namespace RoyAppMaui.Services;
public interface IFileService
{
    Result<List<Sleep>> GetSleepDataFromCsv(string filePath);
    Task<bool> SaveDataToFile(IEnumerable<Sleep> sleeps);
    Task<FileResult?> SelectImportFile();
}
