namespace PlasterSkull.Framework.Blazor;

internal class PsDialogReferenceListenerWrapper
{
    public required IDialogReference DialogReference { get; init; }
    public bool IsBackButtonObserver { get; set; }
}
