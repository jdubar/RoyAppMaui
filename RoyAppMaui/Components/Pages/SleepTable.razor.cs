using MudBlazor;

using RoyAppMaui.Models;

using System.Collections.ObjectModel;
using System.Globalization;

namespace RoyAppMaui.Components.Pages;
public partial class SleepTable
{
    private readonly ObservableCollection<Sleep> _items = [];
    private Sleep _sleep = new();
    private MudTimePicker _bedtimepicker = new();
    private MudTimePicker _waketimepicker = new();

    void AddNewItem() =>
        _items.Add(new Sleep());

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

    private void RemoveItem(CellContext<Sleep> item)
    {
        var index = _items.IndexOf(item.Item);
        _items.RemoveAt(index);
    }

    private void StartedEditingItem(Sleep item) =>
        _sleep = item;

    private static decimal GetDuration(decimal bedtime, decimal waketime)
    {
        var duration = waketime - bedtime;
        return duration > 0
            ? duration
            : 24 + duration;
    }
}
