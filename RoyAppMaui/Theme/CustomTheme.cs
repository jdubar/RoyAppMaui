namespace RoyAppMaui.Theme;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "No need to test this theme file. There is no logic.")]
public class CustomTheme : MudTheme
{
    public CustomTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Tertiary = "#594ae2",
        };
        PaletteDark = new PaletteDark()
        {
            Tertiary = "#ffffffb2",
        };
    }
}
