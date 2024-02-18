namespace RoyAppMaui.Services;
public interface IDateTimeService
{
    TimeSpan StringToTimeSpan(string time);
    string TimeSpanToDateTime(TimeSpan newTime);
    decimal TimeSpanToDecimal(TimeSpan? newTime);
}
