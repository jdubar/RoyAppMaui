using FluentResults;

using RoyAppMaui.Models;

namespace RoyAppMaui.Services;
public interface IFileService
{
    Result<List<Sleep>> GetSleepDataFromCsv(string filePath);
    Task<Result<bool>> SaveBytesToFileAsync(byte[] buffer, string filePath);
    Task<Result<string>> SelectImportFileAsync();
}
