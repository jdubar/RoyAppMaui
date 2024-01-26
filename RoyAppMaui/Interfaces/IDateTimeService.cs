namespace RoyAppMaui.Interfaces;
public interface IDateTimeService
{
    decimal GetDuration(decimal bedtime, decimal waketime);
    TimeSpan StringToTimeSpan(string time);
    string TimeSpanToDateTime(TimeSpan newTime);
    decimal TimeSpanToDecimal(TimeSpan? newTime);
}
