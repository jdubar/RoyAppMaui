using FluentResults;

using RoyApp.Models;

namespace RoyApp.Services;
public interface IFileService
{
    Result<List<Sleep>> GetSleepDataFromCsv(string filePath);
    Task<Result> SaveBytesToFileAsync(byte[] buffer, string filePath);
    Task<Result<string>> SelectImportFileAsync();
}
