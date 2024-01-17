using MudBlazor;

using RoyAppMaui.Models;

using System.Collections.ObjectModel;
using System.Globalization;

namespace RoyAppMaui.Components.Pages;
public partial class SleepTable
{
    private readonly ObservableCollection<Sleep> _items = [];
    private Sleep _sleep = new();
    private MudTimePicker? _bedtimepicker;
    private MudTimePicker? _waketimepicker;

    private void Edit(Sleep sleep)
    {
        _sleep = sleep;
    }

    private void Save()
    {
        // TODO: Still trying to figure out editing vs adding items
        var newsleep = new Sleep()
        {
            Id = _sleep.Id,
            Bedtime = _sleep.Bedtime,
            BedtimeRec = _sleep.BedtimeRec,
            BedtimeDisplay = DateTime.Today.Add((TimeSpan)_sleep.Bedtime).ToString("hh:mm tt"),
            Waketime = _sleep.Waketime,
            WaketimeRec = _sleep.WaketimeRec,
            WaketimeDisplay = DateTime.Today.Add((TimeSpan)_sleep.Waketime).ToString("hh:mm tt"),
            Duration = GetDuration(_sleep.BedtimeRec, _sleep.WaketimeRec)
        };
        if (_sleep.Guid == Guid.Empty)
        {
            _items.Add(newsleep);
        }
        else
        {
            var index = _items.IndexOf(_sleep);
            _items.RemoveAt(index);
            _items.Insert(index, newsleep);
        }
        _sleep = new Sleep();
    }

    private void HandleBedTimeChange(TimeSpan? newTime)
    {
        if (newTime == null)
        {
            return;
        }
        _sleep.Bedtime = newTime;
        _sleep.BedtimeRec = decimal.Round(Convert.ToDecimal(TimeSpan.Parse(newTime.ToString() ?? "0:0", CultureInfo.InvariantCulture).TotalHours), 2);
    }

    private void HandleWakeTimeChange(TimeSpan? newTime)
    {
        if (newTime == null)
        {
            return;
        }
        _sleep.Waketime = newTime;
        _sleep.WaketimeRec = decimal.Round(Convert.ToDecimal(TimeSpan.Parse(newTime.ToString() ?? "0:0", CultureInfo.InvariantCulture).TotalHours), 2);
    }

    private void ResetBedTimeValidation() =>
        _bedtimepicker?.ResetValidation();

    private void ResetWakeTimeValidation() =>
        _waketimepicker?.ResetValidation();

    private static decimal GetDuration(decimal bedtime, decimal waketime)
    {
        var duration = waketime - bedtime;
        return duration > 0
            ? duration
            : 24 + duration;
    }
}
