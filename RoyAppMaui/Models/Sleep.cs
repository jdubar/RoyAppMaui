namespace RoyAppMaui.Models;
public class Sleep
{
    public int Id { get; set; }
    public TimeSpan? Bedtime { get; set; } = new TimeSpan(0, 0, 0);
    public decimal BedtimeRec { get; set; }
    public TimeSpan? Waketime { get; set; } = new TimeSpan(0, 0, 0);
    public decimal WaketimeRec { get; set; }
    public decimal Duration { get; set; }
}
