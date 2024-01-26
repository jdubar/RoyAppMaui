using Microsoft.AspNetCore.Components;

using MudBlazor;

using RoyAppMaui.Components.Modals;
using RoyAppMaui.Interfaces;
using RoyAppMaui.Models;
using RoyAppMaui.Services;

using System.Collections.ObjectModel;

namespace RoyAppMaui.Components.Pages;
public partial class SleepTable
{
    [Inject] private IDateTimeService DateTimeService { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private IFileService FileService { get; set; } = default!;
    [Inject] private NotifyStateService NotifyService { get; set; } = default!;

    private ObservableCollection<Sleep> _items = [];
    private Sleep _sleep = new();
    private MudTimePicker _bedtimepicker = new();
    private MudTimePicker _waketimepicker = new();

    private decimal BedtimeAvg {  get; set; }
    private decimal WaketimeAvg {  get; set; }

    protected override void OnInitialized()
    {
        NotifyService.EventClick += OnFileImportClick;
        base.OnInitialized();
    }

    private void AddNewItem() =>
        _items.Add(new Sleep());

    private void ClearTable() =>
        _items.Clear();

    private void HandleTimeChange(MudTimePicker timepicker, TimeSpan? newTime)
    {
        if (newTime == null || timepicker == null)
        {
            return;
        }
        if (timepicker == _bedtimepicker)
        {
            SetBedtimeModelInfo((TimeSpan)newTime);
        }
        else
        {
            SetWaketimeModelInfo((TimeSpan)newTime);
        }
        _sleep.Duration = DateTimeService.GetDuration(_sleep.BedtimeRec, _sleep.WaketimeRec);
    }

    private async Task ImportFileData()
    {
        var selectedFile = await FileService.SelectImportFile();
        if (selectedFile != null && selectedFile.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
        {
            ClearTable();
            _items = FileService.ParseImportFileData(selectedFile.FullPath);
            SetAveragesInView();
            await InvokeAsync(StateHasChanged);
        }
    }

    private void OnCommittedItemChanges(Sleep sleep) =>
        SetAveragesInView();

    private void OnFileImportClick(object? sender, EventArgs e)
    {
        _ = Task.Run(ImportFileData);
        InvokeAsync(StateHasChanged);
    }

    private void OnStartedEditingItem(Sleep item) =>
        _sleep = item;

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
        if (_items.Count == 0)
        {
            BedtimeAvg = 0;
            WaketimeAvg = 0;
            return;
        }
        BedtimeAvg = decimal.Round(_items.Sum(s => s.BedtimeRec) / _items.Count, 2);
        WaketimeAvg = decimal.Round(_items.Sum(s => s.WaketimeRec) / _items.Count, 2);
    }

    private void SetBedtimeModelInfo(TimeSpan timeSpan)
    {
        _sleep.Bedtime = timeSpan;
        _sleep.BedtimeRec = DateTimeService.TimeSpanToDecimal(timeSpan);
        _sleep.BedtimeDisplay = DateTimeService.TimeSpanToDateTime(timeSpan);
    }

    private void SetWaketimeModelInfo(TimeSpan timeSpan)
    {
        _sleep.Waketime = timeSpan;
        _sleep.WaketimeRec = DateTimeService.TimeSpanToDecimal(timeSpan);
        _sleep.WaketimeDisplay = DateTimeService.TimeSpanToDateTime(timeSpan);
    }

    private async Task<DialogResult> ShowConfirmDeleteDialogAsync()
    {
        var parameters = new DialogParameters<ConfirmDelete>
        {
            { x => x.ContentText, "Do you really want to delete this item? This process cannot be undone." }
        };

        var options = new DialogOptions()
        {
            CloseButton = true,
            MaxWidth = MaxWidth.ExtraSmall
        };

        var confirmModal = DialogService.Show<ConfirmDelete>("Delete", parameters, options);
        return await confirmModal.Result;
    }
}
