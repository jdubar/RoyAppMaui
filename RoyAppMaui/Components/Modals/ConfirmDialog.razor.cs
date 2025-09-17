namespace RoyAppMaui.Components.Modals;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class ConfirmDialog
{
    [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public string DialogText { get; set; } = string.Empty;
    [Parameter] public string ButtonText { get; set; } = "Confirm";
    [Parameter] public MudBlazor.Color ButtonColor { get; set; } = MudBlazor.Color.Primary;

    private void Cancel() => MudDialog.Cancel();
    private void Submit() => MudDialog.Close(DialogResult.Ok(true));
}
