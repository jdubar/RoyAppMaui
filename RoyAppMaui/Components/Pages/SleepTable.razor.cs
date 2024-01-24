using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic.FileIO;

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

    private readonly ObservableCollection<Sleep> _items = [];
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
            SetBedtimeModelInfo(ref _sleep, (TimeSpan)newTime);
        }
        else
        {
            SetWaketimeModelInfo(ref _sleep, (TimeSpan)newTime);
        }
        SetDuration(ref _sleep, _sleep.BedtimeRec, _sleep.WaketimeRec);
    }

    private async Task ImportFileData()
    {
        var selectedFile = await FileService.SelectImportFile();
        if (selectedFile != null && selectedFile.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
        {
            ClearTable();
            using (var parser = new TextFieldParser(selectedFile.FullPath))
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    var sleep = new Sleep();
                    var fields = parser.ReadFields();
                    if (fields != null && fields.Length == 3)
                    {
                        sleep.Id = fields[0];
                        SetBedtimeModelInfo(ref sleep, DateTimeService.StringToTimeSpan(fields[1]));
                        SetWaketimeModelInfo(ref sleep, DateTimeService.StringToTimeSpan(fields[2]));
                        SetDuration(ref sleep, sleep.BedtimeRec, sleep.WaketimeRec);
                        _items.Add(sleep);
                    }
                }
            }
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

    private void SetBedtimeModelInfo(ref Sleep sleep, TimeSpan timeSpan)
    {
        sleep.Bedtime = timeSpan;
        sleep.BedtimeRec = DateTimeService.TimeSpanToDecimal(timeSpan);
        sleep.BedtimeDisplay = DateTimeService.TimeSpanToDateTime(timeSpan);
    }

    private static void SetDuration(ref Sleep sleep, decimal bedtime, decimal waketime)
    {
        var duration = waketime - bedtime;
        sleep.Duration = duration > 0
                       ? duration
                       : 24 + duration;
    }

    private void SetWaketimeModelInfo(ref Sleep sleep, TimeSpan timeSpan)
    {
        sleep.Waketime = timeSpan;
        sleep.WaketimeRec = DateTimeService.TimeSpanToDecimal(timeSpan);
        sleep.WaketimeDisplay = DateTimeService.TimeSpanToDateTime(timeSpan);
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
