namespace RoyAppMaui.Models;
public class Sleep
{
    public int Id { get; set; }
    public TimeSpan? Bedtime { get; set; } = new TimeSpan(0, 0, 0);
    public decimal BedtimeRec { get; set; }
    public string BedtimeDisplay { get; set; } = "12:00 AM";
    public TimeSpan? Waketime { get; set; } = new TimeSpan(0, 0, 0);
    public decimal WaketimeRec { get; set; }
    public string WaketimeDisplay { get; set; } = "12:00 AM";
    public decimal Duration { get; set; }
}
