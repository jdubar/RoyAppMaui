namespace RoyAppMaui.Interfaces;
public interface ISettingsService
{
    bool IsDarkMode { get; set; }
    int RowsPerPage { get; set; }
}
