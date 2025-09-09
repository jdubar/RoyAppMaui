namespace RoyAppMaui.Theme;
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
