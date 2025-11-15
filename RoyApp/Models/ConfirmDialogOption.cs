namespace RoyApp.Models;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "No need to test this model. There is no logic.")]
public record ConfirmDialogOption
{
    public string DialogText { get; init; } = string.Empty;
    public string Action { get; init; } = "Confirm";
    public MudBlazor.Color ButtonColor { get; init; } = MudBlazor.Color.Primary;
}
