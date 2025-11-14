using RoyApp.Extensions;
using RoyApp.Models;
using RoyApp.Services;

using System.Collections.ObjectModel;

namespace RoyApp.Components.Pages;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class SleepTable
{
    [Inject] private ICustomDialogService DialogService { get; set; } = default!;
    [Inject] private IDataService DataService { get; set; } = default!;
    [Inject] private IFileService FileService { get; set; } = default!;
    [Inject] private IImportExportService ImportExportService { get; set; } = default!;
    [Inject] private ISettingsService SettingsService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private ObservableCollection<Sleep> _sleeps = [];
    private bool _isLoading;
    private bool _drawerOpen;

    const string _zeroTimeString = "0";

    public string BedtimeAverage { get; private set; } = _zeroTimeString;
    public string WaketimeAverage { get; private set; } = _zeroTimeString;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        ImportExportService.OnExportRequested += async () => await ExportTableDataAsync();
        ImportExportService.OnImportRequested += async () => await ImportDataAsync();
    }

    private async Task AddItemAsync()
    {
        var result = await DialogService.ShowModifySleepItemDialogAsync(new Sleep(), "Add Sleep Entry");
        if (result.IsFailed)
        {
            return;
        }

        _sleeps.Add(result.Value);
        SetAveragesInView();
    }

    private async Task ConfirmClearTableAsync()
    {
        var result = await DialogService.ShowConfirmClearGridDialogAsync();
        if (result.IsFailed)
        {
            return;
        }

        ClearTable();
    }

    private void ClearTable()
    {
        _sleeps.Clear();
        ResetAverages();
    }

    private static string GetDurationColumnClass()
    {
#if WINDOWS
        return "duration-column";
#else
        return string.Empty;
#endif
    }

    private static string GetActionColumnClass()
    {
#if WINDOWS
        return "action-column";
#else
        return string.Empty;
#endif
    }

    private async Task ImportDataAsync()
    {
        var result = await FileService.SelectImportFileAsync();
        if (result.IsFailed)
        {
            if (!result.HasError<UserCanceledError>())
            {
                _ = Snackbar.Add(result.Errors[0].Message, Severity.Error);
            }
            return;
        }

        var filePath = result.Value;
        await ImportFileDataAsync(filePath);
    }

    private async Task ImportFileDataAsync(string filePath)
    {
        _isLoading = true;
        ClearTable();

        try
        {
            var result = FileService.GetSleepDataFromCsv(filePath);
            if (result.IsFailed)
            {
                _ = Snackbar.Add(result.Errors[0].Message, Severity.Error);
                return;
            }

            _sleeps = new ObservableCollection<Sleep>(result.Value);
            SetAveragesInView();
        }
        finally
        {
            _isLoading = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task ModifyItemAsync(Sleep sleep)
    {
        var result = await DialogService.ShowModifySleepItemDialogAsync(sleep, "Edit Sleep Entry");
        if (result.IsFailed)
        {
            return;
        }

        var index = _sleeps.IndexOf(result.Value);
        _sleeps[index] = result.Value;
        SetAveragesInView();
    }

    private void OnRowsPerPageChanged(int pageSize) => SettingsService.RowsPerPage = pageSize;

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }

    private async Task ExportTableDataAsync()
    {
        if (_sleeps.Count < 1)
        {
            _ = Snackbar.Add("There are no items to export", Severity.Info);
            return;
        }

        var filename = $"RoyApp_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
        var data = DataService.GetExportData(_sleeps.AsEnumerable());
        var result = await FileService.SaveBytesToFileAsync(data, filename);
        if (result.IsFailed)
        {
            if (!result.HasError<UserCanceledError>())
            {
                Snackbar.Add("Error saving the file!", Severity.Error);
            }
            return;
        }

        Snackbar.Add("Successfully exported the data to file", Severity.Success);
    }

    private async Task RemoveItemAsync(Sleep item)
    {
        var result = await DialogService.ShowDeleteItemDialogAsync();
        if (result.IsFailed)
        {
            return;
        }

        var index = _sleeps.IndexOf(item);
        _sleeps.RemoveAt(index);
        SetAveragesInView();
    }

    private void ResetAverages()
    {
        BedtimeAverage = _zeroTimeString;
        WaketimeAverage = _zeroTimeString;
    }

    private void SetAveragesInView()
    {
        if (_sleeps.Count < 1)
        {
            ResetAverages();
            return;
        }

        BedtimeAverage = _sleeps.GetAverage(s => s.BedtimeAsDecimal).ToFormattedString();
        WaketimeAverage = _sleeps.GetAverage(s => s.WaketimeAsDecimal).ToFormattedString();
    }
}
