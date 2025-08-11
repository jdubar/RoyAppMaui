using CommunityToolkit.Maui.Storage;

using RoyAppMaui.Components.Modals;
using RoyAppMaui.Enums;
using RoyAppMaui.Extensions;
using RoyAppMaui.Models;
using RoyAppMaui.Services;
using RoyAppMaui.Types;

using System.Collections.ObjectModel;

namespace RoyAppMaui.Components.Pages;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class SleepTable
{
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private IFileService FileService { get; set; } = default!;
    [Inject] private IFilePicker FilePicker { get; set; } = default!;
    [Inject] private IFileSaver FileSaver { get; set; } = default!;
    [Inject] private IImportExportService ImportExportService { get; set; } = default!;
    [Inject] private ISettingsService Settings { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private ObservableCollection<Sleep> _items = [];
    private Sleep _selectedItem = new();

    private MudForm itemForm = new();
    private bool isValidItem = true;

    private bool _isLoading;
    private bool _showModifyItemDialog;

    private decimal BedtimeAvg {  get; set; }
    private decimal WaketimeAvg {  get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        ImportExportService.OnExportRequested += async () => await ExportTableDataAsync();
        ImportExportService.OnImportRequested += async () => await ImportDataAsync();
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

    private void ShowAddNewItemOverlay()
    {
        _showModifyItemDialog = true;
        _selectedItem = new Sleep();
    }

    private void ClearTable()
    {
        _items.Clear();
        BedtimeAvg = 0;
        WaketimeAvg = 0;
    }

    private void HandleTimeChange(TimePickers timepicker, TimeSpan? newTime)
    {
        if (newTime is null)
        {
            return;
        }

        switch (timepicker)
        {
            case TimePickers.Bedtime:
                _selectedItem.Bedtime = (TimeSpan)newTime;
                break;
            case TimePickers.Waketime:
                _selectedItem.Waketime = (TimeSpan)newTime;
                break;
            default:
                return;
        }
    }

    private async Task ImportFileDataAsync(string filePath)
    {
        _isLoading = true;
        ClearTable();

        var result = FileService.GetSleepDataFromCsv(filePath);
        if (result.IsFailed)
        {
            _ = Snackbar.Add(result.Errors[0].Message, Severity.Error);
            _isLoading = false;
            return;
        }

        _items = new ObservableCollection<Sleep>(result.Value);
        SetAveragesInView();

        _isLoading = false;
        await InvokeAsync(StateHasChanged);
    }

    private void OnCloseOverlay() => _showModifyItemDialog = false;

    private void OnCommittedItemChanges(Sleep sleep) => SetAveragesInView();

    private void OnModifyItem(Sleep item)
    {
        _selectedItem = item;
        _showModifyItemDialog = true;
    }

    private void OnSaveBedtimeItem()
    {
        itemForm.Validate();
        if (itemForm.IsValid)
        {
            _showModifyItemDialog = false;
            if (_items.Contains(_selectedItem))
            {
                var index = _items.IndexOf(_selectedItem);
                _items[index] = _selectedItem;
            }
            else
            {
                _items.Add(_selectedItem);
            }

            SetAveragesInView();
        }
    }

    private void OnRowsPerPageChanged(int pageSize) => Settings.RowsPerPage = pageSize;

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
        var result = await ShowConfirmDeleteDialogAsync();
        if (result.Canceled)
        {
            return;
        }

        var index = _items.IndexOf(item);
        _items.RemoveAt(index);
        SetAveragesInView();
    }

    private void SetAveragesInView()
    {
        if (_items.Count < 1)
        {
            BedtimeAvg = 0;
            WaketimeAvg = 0;
            return;
        }

        BedtimeAvg = _items.GetAverage(s => s.BedtimeRec);
        WaketimeAvg = _items.GetAverage(s => s.WaketimeRec);
    }

    private async Task<DialogResult> ShowConfirmDeleteDialogAsync()
    {
        var dialog = await DialogService.ShowAsync<ConfirmDelete>("Delete");
        var result = await dialog.Result;

        return result is null || result.Canceled
            ? DialogResult.Cancel()
            : DialogResult.Ok(true);
    }
}
