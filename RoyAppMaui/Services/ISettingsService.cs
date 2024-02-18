namespace RoyAppMaui.Services;
public interface ISettingsService
{
    bool IsDarkMode { get; set; }
    int RowsPerPage { get; set; }
}
