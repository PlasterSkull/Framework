namespace PlasterSkull.Framework.Blazor;

public class PsBackButtonEventContext
{
    public bool IsLeaveQueueRequested { get; protected internal set; }
    public bool IsCompleteRequested { get; protected internal set; }
    public bool HasErrors { get; set; }

    public void RequestLeaveQueue() =>
        IsLeaveQueueRequested = true;

    public void RequestComplete() =>
        IsCompleteRequested = true;

    internal static PsBackButtonEventContext New() => new();

    internal string GetFinalStateMessage() =>
        $"BackButton event finished" +
        $"{(IsCompleteRequested ? "with success handle" : "")}" +
        $"{(HasErrors ? "with errors" : "")}";
}
