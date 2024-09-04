namespace PlasterSkull.Framework.Blazor;

public interface IMudDialogServiceListener
{
    void Start();
    void RemoveFromBackButtonQueue(Guid dialogRefId);
    void Unsubscribe(Guid dialogRefId);
}
