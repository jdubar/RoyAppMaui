using RoyAppMaui.Enums;
using RoyAppMaui.Models;

namespace RoyAppMaui.Components.Modals;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class AddModifyItem
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public Sleep ItemToModify { get; set; } = new();

    private MudForm _itemForm = new();
    private MudTimePicker _bedtimepicker = default!;
    private MudTimePicker _waketimepicker = default!;

    private void Cancel() => MudDialog.Cancel();

    private void OnTimeChange(TimePickers timepicker, TimeSpan? newTime)
    {
        if (newTime is null)
        {
            return;
        }

        switch (timepicker)
        {
            case TimePickers.Bedtime:
                ItemToModify.Bedtime = (TimeSpan)newTime;
                break;
            case TimePickers.Waketime:
                ItemToModify.Waketime = (TimeSpan)newTime;
                break;
            default:
                return;
        }
    }

    private void Save()
    {
        _itemForm.Validate();
        if (_itemForm.IsValid)
        {
            MudDialog.Close(DialogResult.Ok(ItemToModify));
        }
    }
}
