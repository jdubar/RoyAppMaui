using RoyAppMaui.Components.Modals;
using RoyAppMaui.Enums;
using RoyAppMaui.Models;
using RoyAppMaui.Services;

using System.Collections.ObjectModel;

namespace RoyAppMaui.Components.Pages;
public partial class SleepTable
{
    [Inject] private IDataService DataService { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private IFileService FileService { get; set; } = default!;
    [Inject] private ISettingsService Settings { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private NotifyService NotifyService { get; set; } = default!;

    private ObservableCollection<Sleep> _items = [];
    private Sleep _sleep = new();

    private bool _isLoading;

    private decimal BedtimeAvg {  get; set; }
    private decimal WaketimeAvg {  get; set; }

    protected override void OnInitialized()
    {
        NotifyService.EventClick += OnMenuItemClick;
        base.OnInitialized();
    }

    private void AddNewItem() =>
        _items.Add(new Sleep());

    private void ClearTable()
    {
        _items.Clear();
        BedtimeAvg = 0;
        WaketimeAvg = 0;
    }

    private void HandleTimeChange(TimePickers timepicker, TimeSpan? newTime)
    {
        if (newTime == null)
        {
            return;
        }

        switch (timepicker)
        {
            case TimePickers.Bedtime:
                SetBedtimeModelInfo((TimeSpan)newTime);
                break;
            case TimePickers.Waketime:
                SetWaketimeModelInfo((TimeSpan)newTime);
                break;
            default:
                return;
        }
        _sleep.Duration = DataService.GetDuration(_sleep.BedtimeRec, _sleep.WaketimeRec);
    }

    private async Task ImportFileDataAsync()
    {
        var selectedFile = await FileService.SelectImportFile();
        if (selectedFile != null && selectedFile.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
        {
            _isLoading = true;
            ClearTable();
            _items = FileService.ParseImportFileData(selectedFile.FullPath);
            if (_items is not null)
            {
                if (_items.Count > 0)
                {
                    SetAveragesInView();
                    _isLoading = false;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    _ = Snackbar.Add("No items were imported, check the import file", Severity.Info);
                }
            }
            else
            {
                _ = Snackbar.Add("Error! The items list was null", Severity.Error);
            }
            _isLoading = false;
        }
    }

    private void OnCommittedItemChanges(Sleep sleep) =>
        SetAveragesInView();

    private void OnMenuItemClick(object? sender, EventArgs e)
    {
        switch (((MenuItemClickEventArgs)e).MenuItem)
        {
            case MenuItems.Import:
                _ = Task.Run(ImportFileDataAsync);
                break;
            case MenuItems.Export:
                _ = Task.Run(SaveFileDataAsync);
                break;
            case MenuItems.Clear:
                ClearTable();
                break;
        }
        InvokeAsync(StateHasChanged);
    }

    private void OnRowsPerPageChanged(int pageSize) =>
        Settings.RowsPerPage = pageSize;

    private void OnStartedEditingItem(Sleep item) =>
        _sleep = item;

    private async Task SaveFileDataAsync()
    {
        if (_items.Count < 1)
        {
            _ = Snackbar.Add("There are no items to save", Severity.Info);
            return;
        }

        _ = await FileService.SaveDataToFile(DataService.GetExportData(_items))
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

        BedtimeAvg = DataService.GetAverageOfBedtimes(_items);
        WaketimeAvg = DataService.GetAverageOfWaketimes(_items);
    }

    private void SetBedtimeModelInfo(TimeSpan timeSpan)
    {
        _sleep.Bedtime = timeSpan;
        _sleep.BedtimeRec = DataService.TimeSpanToDecimal(timeSpan);
        _sleep.BedtimeDisplay = DataService.TimeSpanToDateTime(timeSpan);
    }

    private void SetWaketimeModelInfo(TimeSpan timeSpan)
    {
        _sleep.Waketime = timeSpan;
        _sleep.WaketimeRec = DataService.TimeSpanToDecimal(timeSpan);
        _sleep.WaketimeDisplay = DataService.TimeSpanToDateTime(timeSpan);
    }

    private async Task<DialogResult> ShowConfirmDeleteDialogAsync()
    {
        var confirmModal = DialogService.Show<ConfirmDelete>("Delete");
        return await confirmModal.Result;
    }
}
