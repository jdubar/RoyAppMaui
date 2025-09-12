using FluentResults;

using RoyAppMaui.Components.Modals;
using RoyAppMaui.Models;

namespace RoyAppMaui.Services.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "No need to test this service model. There is no logic.")]
public class CustomDialogService(IDialogService dialogService) : ICustomDialogService
{
    public async Task<Result<bool>> ShowConfirmClearGridDialogAsync()
    {
        var option = new ConfirmDialogOption()
        {
            DialogText = "Are you sure you want to clear the grid? This action cannot be undone.",
            Action = "Clear",
            ButtonColor = MudBlazor.Color.Primary
        };

        return await ShowConfirmDialogAsync(option);
    }

    public async Task<Result<bool>> ShowDeleteItemDialogAsync()
    {
        var option = new ConfirmDialogOption()
        {
            DialogText = "Are you sure you want to delete this item? This action cannot be undone.",
            Action = "Delete",
            ButtonColor = MudBlazor.Color.Error
        };

        return await ShowConfirmDialogAsync(option);
    }

    public async Task<Result<Sleep>> ShowModifySleepItemDialogAsync(Sleep sleep, string title)
    {
        var parameters = new DialogParameters<AddModifyItem>
        {
            { x => x.ItemToModify, sleep }
        };
        var dialog = await dialogService.ShowAsync<AddModifyItem>(title, parameters);
        var result = await dialog.Result;

        if (result is null)
        {
            return Result.Fail("Dialog result was null");
        }

        if (result.Canceled)
        {
            return Result.Fail("Dialog was canceled");
        }

        if (result.Data is null)
        {
            return Result.Fail("Dialog data was null");
        }

        return Result.Ok((Sleep)result.Data);
    }

    private async Task<Result<bool>> ShowConfirmDialogAsync(ConfirmDialogOption option)
    {
        var parameters = new DialogParameters<ConfirmDialog>
        {
            { x => x.DialogText, option.DialogText },
            { x => x.ButtonText, option.Action },
            { x => x.ButtonColor, option.ButtonColor }
        };

        var dialog = await dialogService.ShowAsync<ConfirmDialog>(option.Action, parameters);
        var result = await dialog.Result;

        if (result is null)
        {
            return Result.Fail("Dialog result was null");
        }

        if (result.Canceled)
        {
            return Result.Fail("Dialog was canceled");
        }

        return Result.Ok(true);
    }
}
