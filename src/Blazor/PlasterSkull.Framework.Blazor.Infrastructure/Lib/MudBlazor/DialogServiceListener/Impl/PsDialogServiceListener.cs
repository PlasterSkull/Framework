using System.Collections.Concurrent;

namespace PlasterSkull.Framework.Blazor;

internal sealed class PsDialogServiceListener(
    IDialogService _dialogService,
    IPsBackButtonService _backButtonService)
    : IPsDialogServiceListener
    , IPsBackButtonObserver
    , IDisposable
{
    #region Fields

    private readonly TagId _tagId = TagId.NewWithTagFormatting(nameof(PsDialogServiceListener));
    private readonly ConcurrentDictionary<Guid, PsDialogReferenceListenerWrapper> _openedDialogs = [];

    #endregion

    #region External events

    private void OnDialogCloseRequested(IDialogReference dialogRef, DialogResult? _) =>
        Unsubscribe(dialogRef.Id);

    private Task<bool> OnDialogInstanceAddedAsync(IDialogReference dialogRef) =>
        Task.FromResult(_openedDialogs.TryAdd(dialogRef.Id, new PsDialogReferenceListenerWrapper
        {
            DialogReference = dialogRef,
        }));

    #endregion

    #region BackButtonObserver

    TagId IPsBackButtonObserver.TagId => _tagId;

    PsBackButtonObserverInfo IPsBackButtonObserver.BackButtonObserverInfo { get; } = new()
    {
        Priority = PsBackButtonDefaultPriorities.Dialog,
    };

    ValueTask IPsBackButtonObserver.NotifyBackButtonEventAsync(PsBackButtonEventContext context, CancellationToken _)
    {
        if (_openedDialogs.IsEmpty)
            return ValueTask.CompletedTask;

        var dialogRefWrapper = _openedDialogs.Last().Value;
        dialogRefWrapper.DialogReference.Close(DialogResult.Cancel());
        Unsubscribe(dialogRefWrapper.DialogReference.Id);

        context.RequestComplete();
        return ValueTask.CompletedTask;
    }

    #endregion

    public void Start()
    {
        _dialogService.DialogInstanceAddedAsync += OnDialogInstanceAddedAsync;
        _dialogService.OnDialogCloseRequested += OnDialogCloseRequested;
        _backButtonService.Subscribe(this);
    }

    public void Unsubscribe(Guid dialogRefId) =>
        _openedDialogs.TryRemove(dialogRefId, out var _);

    public void RemoveFromBackButtonQueue(Guid dialogRefId)
    {
        if (!_openedDialogs.TryGetValue(dialogRefId, out var dialogRefWrapper))
            return;

        dialogRefWrapper.IsBackButtonObserver = true;
    }

    public void Dispose()
    {
        _dialogService.DialogInstanceAddedAsync -= OnDialogInstanceAddedAsync;
        _dialogService.OnDialogCloseRequested -= OnDialogCloseRequested;
        _backButtonService.Unsubscribe(this);
    }
}
