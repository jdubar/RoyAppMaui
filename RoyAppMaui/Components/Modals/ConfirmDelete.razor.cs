using Microsoft.AspNetCore.Components;

using MudBlazor;

namespace RoyAppMaui.Components.Modals;
public partial class ConfirmDelete
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = new();

    [Parameter] public string ContentText { get; set; } = string.Empty;

    private void Cancel() =>
        MudDialog.Cancel();

    private void Submit() =>
        MudDialog.Close(DialogResult.Ok(true));
}
