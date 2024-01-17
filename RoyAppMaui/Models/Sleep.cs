namespace RoyAppMaui.Models;
public record Sleep
{
    public int Id { get; set; }
    public TimeSpan? Bedtime { get; set; } = new TimeSpan(0, 0, 0);
    public decimal BedtimeRec { get; set; }
    public string BedtimeDisplay { get; set; } = string.Empty;
    public TimeSpan? Waketime { get; set; } = new TimeSpan(0, 0, 0);
    public decimal WaketimeRec { get; set; }
    public string WaketimeDisplay { get; set; } = string.Empty;
    public decimal Duration { get; set; }
}
