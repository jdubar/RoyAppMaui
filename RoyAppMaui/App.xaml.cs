using RoyAppMaui.Services;

namespace RoyAppMaui;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the app code behind. There's no logic to test.")]
public partial class App : Application
{
    private readonly ISettingsService _settingsService;

    public App(ISettingsService settingsService)
    {
        InitializeComponent();
        _settingsService = settingsService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new MainPage());

#if WINDOWS
        window.MinimumWidth = 1000;
        window.MinimumHeight = 500;

        window.Width = _settingsService.WindowWidth;
        window.Height = _settingsService.WindowHeight;

        window.SizeChanged += (sender, args) =>
        {
            if (sender is Window updatedWindow)
            {
                _settingsService.WindowWidth = updatedWindow.Width;
                _settingsService.WindowHeight = updatedWindow.Height;
            }
        };
#endif
        return window;
    }
}
