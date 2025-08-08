using RoyAppMaui.Services;

namespace RoyAppMaui.Components.Layout;
public partial class MainLayout
{
    [Inject] private IFileService FileService { get; set; } = default!;
    [Inject] private ISettingsService Settings { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private MudThemeProvider _mudThemeProvider = default!;

    private string ThemeIcon => Settings.IsDarkMode
                             ? Icons.Material.Outlined.DarkMode
                             : Icons.Material.Outlined.LightMode;
    private MudBlazor.Color ThemeIconColor => Settings.IsDarkMode
                                           ? MudBlazor.Color.Inherit
                                           : MudBlazor.Color.Warning;

    public string ImportFilePath { get; set; } = default!;
    public bool IsClearDataGrid { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await _mudThemeProvider.WatchSystemDarkModeAsync(OnSystemPreferenceChanged);
            StateHasChanged();
        }
    }

    private void OnClearGridClick() => IsClearDataGrid = true;

    private void OnExportDataClick() => FileService.RequestExport();

    private async Task OnImportDataClick()
    {
        var result = await FileService.SelectImportFile();
        if (result is null)
        {
            return;
        }

        if (!result.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
        {
            _ = Snackbar.Add("The selected file is not a CSV file!", Severity.Error);
            return;
        }

        ImportFilePath = result.FullPath;
    }

    private Task OnSystemPreferenceChanged(bool newValue)
    {
        Settings.IsDarkMode = newValue;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private void ThemeToggle() => Settings.IsDarkMode = !Settings.IsDarkMode;
}
