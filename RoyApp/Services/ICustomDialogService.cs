using FluentResults;

using RoyApp.Models;

namespace RoyApp.Services;
public interface ICustomDialogService
{
    Task<Result<bool>> ShowConfirmClearGridDialogAsync();
    Task<Result<bool>> ShowDeleteItemDialogAsync();
    Task<Result<Sleep>> ShowModifySleepItemDialogAsync(Sleep sleep, string title);
}
