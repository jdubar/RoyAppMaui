using CommunityToolkit.Maui.Storage;

using Microsoft.JSInterop;

using RoyAppMaui.Components.Modals;
using RoyAppMaui.Extensions;
using RoyAppMaui.Models;
using RoyAppMaui.Services;
using RoyAppMaui.Types;

using System.Collections.ObjectModel;

namespace RoyAppMaui.Components.Layout;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class MainLayout
{
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private IFileService FileService { get; set; } = default!;
    [Inject] private IFilePicker FilePicker { get; set; } = default!;
    [Inject] private IFileSaver FileSaver { get; set; } = default!;
    [Inject] private IImportExportService ImportExportService { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private ISettingsService SettingsService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private MudThemeProvider _mudThemeProvider = default!;

    private ObservableCollection<Sleep> _items = [];
    private bool _isLoading;

    public decimal BedtimeAvg {  get; private set; }
    public decimal WaketimeAvg {  get; private set; }

    private string ThemeIcon => SettingsService.IsDarkMode
                             ? Icons.Material.Outlined.DarkMode
                             : Icons.Material.Outlined.LightMode;
    private MudBlazor.Color ThemeIconColor => SettingsService.IsDarkMode
                                           ? MudBlazor.Color.Inherit
                                           : MudBlazor.Color.Warning;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        ImportExportService.OnExportRequested += async () => await ExportTableDataAsync();
        ImportExportService.OnImportRequested += async () => await ImportDataAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && _mudThemeProvider is not null)
        {
            await _mudThemeProvider.WatchSystemDarkModeAsync(OnSystemPreferenceChanged);
            StateHasChanged();
        }
    }

    private async Task ImportDataAsync()
    {
        var result = await SelectImportFile();
        if (result is null)
        {
            return;
        }

        if (!result.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
        {
            _ = Snackbar.Add("The selected file is not a CSV file!", Severity.Error);
            return;
        }

        await ImportFileDataAsync(result.FullPath);
    }


    private void OnExportDataClick() => ImportExportService.RequestExport();

    private void OnImportDataClick() => ImportExportService.RequestImport();

    private Task OnSystemPreferenceChanged(bool newValue)
    {
        SettingsService.IsDarkMode = newValue;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task ThemeToggleAsync()
    {
        SettingsService.IsDarkMode = !SettingsService.IsDarkMode;
        await JS.InvokeVoidAsync("setIsDarkModeCookie", SettingsService.IsDarkMode);
    }

        private async Task AddItemAsync()
    {
        var parameters = new DialogParameters<AddModifyItem>
        {
            { x => x.ItemToModify, new Sleep() }
        };
        var dialog = await DialogService.ShowAsync<AddModifyItem>("Add Sleep Entry", parameters);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            if (result.Data is null)
            {
                return;
            }

            var item = (Sleep)result.Data;
            _items.Add(item);

            SetAveragesInView();
        }
    }

    private void ClearTable()
    {
        _items.Clear();
        ResetAverages();
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

            _items = new ObservableCollection<Sleep>(result.Value);
            SetAveragesInView();
        }
        finally
        {
            _isLoading = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    private void OnCommittedItemChanges(Sleep sleep) => SetAveragesInView();

    private async Task ModifyItemAsync(Sleep item)
    {
        var parameters = new DialogParameters<AddModifyItem>
        {
            { x => x.ItemToModify, item }
        };
        var dialog = await DialogService.ShowAsync<AddModifyItem>("Edit Sleep Entry", parameters);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            if (result.Data is null)
            {
                return;
            }

            var modifiedItem = (Sleep)result.Data;
            var index = _items.IndexOf(modifiedItem);
            _items[index] = modifiedItem;

            SetAveragesInView();
        }
    }

    private void OnRowsPerPageChanged(int pageSize) => SettingsService.RowsPerPage = pageSize;

    private async Task ExportTableDataAsync()
    {
        if (_items.Count < 1)
        {
            _ = Snackbar.Add("There are no items to export", Severity.Info);
            return;
        }

        var data = FileService.GetExportData(_items.AsEnumerable());
        _ = await SaveAsync(data.ToBytes())
            ? Snackbar.Add("Successfully exported the data to file", Severity.Success)
            : Snackbar.Add("Error saving the file!", Severity.Error);
    }

    private async Task<bool> SaveAsync(byte[] data)
    {
        var now = DateTime.Now;
        using var stream = new MemoryStream(data);
        var result = await FileSaver.SaveAsync($"RoyApp_{now:yyyy-MM-dd}_{now:hh:mm:ss}.csv", stream);
        return result.IsSuccessful;
    }

    private async Task<FileResult?> SelectImportFile()
    {
        PickOptions options = new()
        {
            PickerTitle = "Please select an import file",
            FileTypes = FilePickerTypes.GetFilePickerFileTypes()
        };
        var file = await FilePicker.PickAsync(options);
        return file;
    }

    private async Task RemoveItemAsync(Sleep item)
    {
        var result = await ShowDeleteDialogAsync();
        if (result.Canceled)
        {
            return;
        }

        var index = _items.IndexOf(item);
        _items.RemoveAt(index);
        SetAveragesInView();
    }

    private void ResetAverages()
    {
        BedtimeAvg = 0;
        WaketimeAvg = 0;
    }

    private void SetAveragesInView()
    {
        if (_items.Count < 1)
        {
            ResetAverages();
            return;
        }

        BedtimeAvg = _items.GetAverage(s => s.BedtimeRec);
        WaketimeAvg = _items.GetAverage(s => s.WaketimeRec);
    }

    private async Task<DialogResult> ShowDeleteDialogAsync()
    {
        var dialog = await DialogService.ShowAsync<ConfirmDelete>("Delete");
        var result = await dialog.Result;

        return result is null || result.Canceled
            ? DialogResult.Cancel()
            : DialogResult.Ok(true);
    }

}
