namespace RoyAppMaui.Components.Controls;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class AverageField
{
    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public decimal Value { get; set; }
}
