using FluentResults;

using RoyAppMaui.Models;

namespace RoyAppMaui.Services;
public interface IFileService
{
    Result<List<Sleep>> GetSleepDataFromCsv(string filePath);
    byte[] GetExportData(IEnumerable<Sleep> sleeps);
}
