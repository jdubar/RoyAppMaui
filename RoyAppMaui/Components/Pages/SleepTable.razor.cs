using RoyAppMaui.Components.Modals;
using RoyAppMaui.Enums;
using RoyAppMaui.Extensions;
using RoyAppMaui.Models;
using RoyAppMaui.Services;
using RoyAppMaui.Utilities;

using System.Collections.ObjectModel;

namespace RoyAppMaui.Components.Pages;
public partial class SleepTable
{
    [CascadingParameter] public string ImportFilePath { get; set; } = string.Empty;
    [CascadingParameter] public bool IsClearDataGrid { get; set; }
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private IFileService FileService { get; set; } = default!;
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
        FileService.OnExportRequested += async () => await ExportTableDataAsync();
    }

    protected override Task OnParametersSetAsync()
    {
        if (ImportFilePath is not null)
        {
            _ = ImportFileDataAsync(ImportFilePath);
            ImportFilePath = string.Empty;
        }

        if (IsClearDataGrid)
        {
            ClearTable();
            IsClearDataGrid = false;
        }
        return base.OnParametersSetAsync();
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
                _selectedItem.Bedtime = newTime;
                break;
            case TimePickers.Waketime:
                _selectedItem.Waketime = newTime;
                break;
            default:
                return;
        }
        _selectedItem.Duration = SleepUtilities.GetSleepDuration(_selectedItem.BedtimeRec, _selectedItem.WaketimeRec);
    }

    private async Task ImportFileDataAsync(string filePath)
    {
        _isLoading = true;
        ClearTable();

        _items = FileService.ParseImportFileData(filePath);
        if (_items is null)
        {
            _ = Snackbar.Add("Error! The items list was null", Severity.Error);
            _isLoading = false;
            return;
        }

        if (_items.Count > 0)
        {
            SetAveragesInView();
        }
        else
        {
            _ = Snackbar.Add("No items were imported, check the import file", Severity.Info);
        }

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
            _ = Snackbar.Add("There are no items to save", Severity.Info);
            return;
        }

        var data = FileService.GetExportData(_items);
        _ = await FileService.SaveDataToFile(data)
            ? Snackbar.Add("Successfully saved the file", Severity.Success)
            : Snackbar.Add("Error saving the file!", Severity.Error);
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
