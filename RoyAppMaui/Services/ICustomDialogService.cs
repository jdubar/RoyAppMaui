using FluentResults;

using RoyAppMaui.Models;

namespace RoyAppMaui.Services;
public interface ICustomDialogService
{
    Task<Result<bool>> ShowConfirmClearGridDialogAsync();
    Task<Result<bool>> ShowDeleteItemDialogAsync();
    Task<Result<Sleep>> ShowModifySleepItemDialogAsync(Sleep sleep, string title);
}
