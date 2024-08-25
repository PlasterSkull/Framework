using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace PlasterSkull.Framework.Blazor;

internal sealed class PsBackButtonService(
    ILogger<PsBackButtonService> _logger,
    IPsBackButtonClickNativeHandler _backButtonClickNativeHandler)
    : IPsBackButtonService
    , IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly ConcurrentDictionary<TagId, IPsBackButtonObserver> _backButtonObservers = [];

    public void Subscribe(IPsBackButtonObserver observer) =>
        _backButtonObservers.TryAdd(observer.TagId, observer);

    public void Unsubscribe(IPsBackButtonObserver observer) =>
        _backButtonObservers.TryRemove(observer.TagId, out var _);

    public void Unsubscribe(TagId tagId) =>
        _backButtonObservers.TryRemove(tagId, out var _);

    public async ValueTask<PsBackButtonEventContext> PushAsync(CancellationToken ct = default)
    {
        var context = PsBackButtonEventContext.New();

        try
        {
            await _semaphore.WaitAsync(ct);

            if (_backButtonObservers.IsEmpty)
                return await ExecuteAsync();

            foreach (var observerNode in _backButtonObservers.OrderByDescending(x => x.Value.BackButtonObserverInfo.Priority).ToArray())
            {
                var observer = observerNode.Value;
                await observer.NotifyBackButtonEventAsync(context, ct);
                if (context.IsLeaveQueueRequested)
                {
                    Unsubscribe(observer);
                    context.IsLeaveQueueRequested = true;
                }

                if (context.IsCompleteRequested)
                {
                    break;
                }
            }

            return await ExecuteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to handle back button click");
            context.HasErrors = true;
            return context;
        }
        finally
        {
            _semaphore.Release();
        }

        async Task<PsBackButtonEventContext> ExecuteAsync()
        {
            await _backButtonClickNativeHandler.HandleAsync(context, ct);

            _logger.LogDebug("{StateMessage}", context.GetFinalStateMessage());
            return context;
        }
    }

    public void Dispose()
    {
        _semaphore.DisposeSilently();
        _backButtonObservers.Clear();
    }
}