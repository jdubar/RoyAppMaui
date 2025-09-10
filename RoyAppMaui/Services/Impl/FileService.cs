using CommunityToolkit.Maui.Storage;

using CsvHelper;
using CsvHelper.Configuration;

using FluentResults;

using RoyAppMaui.ClassMaps;
using RoyAppMaui.Models;
using RoyAppMaui.Types;

using System.Globalization;

namespace RoyAppMaui.Services.Impl;
public class FileService(
    System.IO.Abstractions.IFileSystem fileSystem,
    IFileSaver fileSaver,
    IFilePicker filePicker) : IFileService
{
    private readonly CancellationTokenSource _cts = new();

    private readonly PickOptions PickOptions = new()
    {
        PickerTitle = "Please select an import file",
        FileTypes = FilePickerTypes.GetFilePickerFileTypes()
    };

    public Result<List<Sleep>> GetSleepDataFromCsv(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return Result.Fail("File path is null or empty.");
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false
        };

        try
        {
            using var reader = fileSystem.File.OpenText(filePath);
            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<SleepMap>();
            var records = csv.GetRecords<Sleep>().ToList();

            return records.Count > 0
                ? Result.Ok(records)
                : Result.Fail("No sleep records found in the CSV file.");
        }
        catch (FileNotFoundException)
        {
            return Result.Fail($"The file at {filePath} was not found.");
        }
        catch (CsvHelperException ex)
        {
            return Result.Fail($"CSV parsing error: {ex.Message}");
        }
        catch (Exception ex)
        {
            return Result.Fail($"An unexpected error occurred while reading the file: {ex.Message}");
        }
    }

    public async Task<Result<bool>> SaveBytesToFileAsync(byte[] buffer, string filePath)
    {
        try
        {
            using var stream = new MemoryStream(buffer);
            var result = await fileSaver.SaveAsync(filePath, stream, _cts.Token);
            if (result.FilePath is null)
            {
                return Result.Fail("user canceled");
            }

            return result.IsSuccessful
                ? Result.Ok(true)
                : Result.Fail("Failed to save the file.");
        }
        catch (FileSaveException ex)
        {
            return Result.Fail($"File save error: {ex.Message}");
        }
    }

    public async Task<Result<string>> SelectImportFileAsync()
    {
        var result = await filePicker.PickAsync(PickOptions);
        if (result is null)
        {
            return Result.Fail("user canceled");
        }

        if (!fileSystem.File.Exists(result.FullPath))
        {
            return Result.Fail("Selected file does not exist.");
        }

        if (fileSystem.Path.GetExtension(result.FullPath)?.ToLower() != ".csv")
        {
            return Result.Fail("Selected file is not a CSV file.");
        }

        return Result.Ok(result.FullPath);
    }
}
