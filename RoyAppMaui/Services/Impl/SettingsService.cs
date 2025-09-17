namespace RoyAppMaui.Services.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a simple settings service with no complex logic, so we don't need to cover it extensively.")]
internal class SettingsService(IPreferences settings) : ISettingsService
{
    private readonly IPreferences Settings = settings;

    public bool IsDarkMode
    {
        get => Settings.Get(nameof(IsDarkMode), false);
        set => Settings.Set(nameof(IsDarkMode), value);
    }

    public int RowsPerPage
    {
        get => Settings.Get(nameof(RowsPerPage), 25);
        set => Settings.Set(nameof(RowsPerPage), value);
    }

    public double WindowHeight
    {
        get => Settings.Get(nameof(WindowHeight), 900.0);
        set => Settings.Set(nameof(WindowHeight), value);
    }

    public double WindowWidth
    {
        get => Settings.Get(nameof(WindowWidth), 1200.0);
        set => Settings.Set(nameof(WindowWidth), value);
    }
}