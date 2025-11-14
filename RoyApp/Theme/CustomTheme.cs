namespace RoyApp.Theme;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "No need to test this theme file. There is no logic.")]
public class CustomTheme : MudTheme
{
    public CustomTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Dark = "#0000001e",
            Tertiary = "#594ae2",
        };
        PaletteDark = new PaletteDark()
        {
            Dark = "#27272f",
            Tertiary = "#ffffffb2",
        };
    }
}
