namespace RoyAppMaui.Models;
public record ConfirmDialogOption
{
    public string DialogText { get; init; } = string.Empty;
    public string Action { get; init; } = "Confirm";
    public MudBlazor.Color ButtonColor { get; init; } = MudBlazor.Color.Primary;
}
