using Microsoft.AspNetCore.Components;

using MudBlazor;

using RoyAppMaui.Components.Modals;
using RoyAppMaui.Models;
using RoyAppMaui.Services;

using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace RoyAppMaui.Components.Pages;
public partial class SleepTable
{
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private NotifyStateService? Service { get; set; }

    private readonly ObservableCollection<Sleep> _items = [];
    private Sleep _sleep = new();
    private MudTimePicker _bedtimepicker = new();
    private MudTimePicker _waketimepicker = new();

    private decimal BedtimeAvg {  get; set; }
    private decimal WaketimeAvg {  get; set; }

    protected override void OnInitialized()
    {
        Service.EventClick += this.Increment;
        base.OnInitialized();
    }

    private void Increment(object? sender, EventArgs e)
    {
        PickAndShow();
        this.InvokeAsync(StateHasChanged);
    }

    private void AddNewItem() =>
        _items.Add(new Sleep());

    public async Task<FileResult> PickAndShow()
    {
        var customFileType = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // UTType values
                { DevicePlatform.Android, new[] { "application/comics" } }, // MIME type
                { DevicePlatform.WinUI, new[] { ".csv", ".cbz" } }, // file extension
                { DevicePlatform.Tizen, new[] { "*/*" } },
                { DevicePlatform.macOS, new[] { "cbr", "cbz" } }, // UTType values
            });

        PickOptions options = new()
        {
            PickerTitle = "Please select a comic file",
            FileTypes = customFileType,
        };

        try
        {
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null && result.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
            {
                using var stream = await result.OpenReadAsync();
                var image = ImageSource.FromStream(() => stream);
            }

            return result;
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }

        return null;
    }

    private void HandleBedTimeChange(TimeSpan? newTime)
    {
        if (newTime == null)
        {
            return;
        }
        _sleep.Bedtime = newTime;
        _sleep.BedtimeRec = decimal.Round(Convert.ToDecimal(TimeSpan.Parse(newTime.ToString() ?? "0:0", CultureInfo.InvariantCulture).TotalHours), 2);
        _sleep.BedtimeDisplay = DateTime.Today.Add((TimeSpan)_sleep.Bedtime).ToString("hh:mm tt");
        _sleep.Duration = GetDuration(_sleep.BedtimeRec, _sleep.WaketimeRec);
    }

    private void HandleWakeTimeChange(TimeSpan? newTime)
    {
        if (newTime == null)
        {
            return;
        }
        _sleep.Waketime = newTime;
        _sleep.WaketimeRec = decimal.Round(Convert.ToDecimal(TimeSpan.Parse(newTime.ToString() ?? "0:0", CultureInfo.InvariantCulture).TotalHours), 2);
        _sleep.WaketimeDisplay = DateTime.Today.Add((TimeSpan)_sleep.Waketime).ToString("hh:mm tt");
        _sleep.Duration = GetDuration(_sleep.BedtimeRec, _sleep.WaketimeRec);
    }

    private async Task RemoveItem(CellContext<Sleep> item)
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
        var result = await confirmModal.Result;
        if (result.Canceled)
        {
            return;
        }
        var index = _items.IndexOf(item.Item);
        _items.RemoveAt(index);
        GetAverages();
    }

    private void CommittedItemChanges(Sleep sleep) =>
        GetAverages();

    private void StartedEditingItem(Sleep item) =>
        _sleep = item;

    private void GetAverages()
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

    private static decimal GetDuration(decimal bedtime, decimal waketime)
    {
        var duration = waketime - bedtime;
        return duration > 0
            ? duration
            : 24 + duration;
    }
}
