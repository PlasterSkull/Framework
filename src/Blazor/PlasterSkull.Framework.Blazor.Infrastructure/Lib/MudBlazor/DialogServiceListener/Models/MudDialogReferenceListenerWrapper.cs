namespace PlasterSkull.Framework.Blazor;

internal class MudDialogReferenceListenerWrapper
{
    public required IDialogReference DialogReference { get; init; }
    public bool IsBackButtonObserver { get; set; }
}
