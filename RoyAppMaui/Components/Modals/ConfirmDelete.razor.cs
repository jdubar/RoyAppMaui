namespace RoyAppMaui.Components.Modals;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class ConfirmDelete
{
    [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;

    private const string ContentText = "Do you really want to delete this item? This process cannot be undone.";

    private void Cancel() => MudDialog.Cancel();

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));
}
