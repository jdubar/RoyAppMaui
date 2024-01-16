namespace RoyAppMaui.Models;
public class Sleep
{
    public int Id { get; set; }
    public DateTime Bedtime { get; set; }
    public decimal BedtimeRec { get; set; }
    public DateTime Waketime { get; set; }
    public decimal WaketimeRec { get; set; }
    public decimal Duration { get; set; }
}
