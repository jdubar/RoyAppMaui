namespace RoyAppMaui.Services.Impl;
internal class SettingsService(IPreferences settings) : ISettingsService
{
    private readonly IPreferences Settings = settings;

    public bool IsDarkMode
    {
        get => Settings.Get("IsDarkMode", false);
        set => Settings.Set("IsDarkMode", value);
    }

    public int RowsPerPage
    {
        get => Settings.Get("RowsPerPage", 25);
        set => Settings.Set("RowsPerPage", value);
    }
}