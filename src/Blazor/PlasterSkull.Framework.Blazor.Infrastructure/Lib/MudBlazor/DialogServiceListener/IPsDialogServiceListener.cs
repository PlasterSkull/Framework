namespace PlasterSkull.Framework.Blazor;

public interface IPsDialogServiceListener
{
    void Start();
    void RemoveFromBackButtonQueue(Guid dialogRefId);
    void Unsubscribe(Guid dialogRefId);
}
