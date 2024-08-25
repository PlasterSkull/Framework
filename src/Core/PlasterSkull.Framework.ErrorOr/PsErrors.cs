namespace PlasterSkull.Framework.ErrorOr;

public static class PsErrors
{
    public static Error NotInitialized(Dictionary<string, object>? metadata = null) =>
        Error.Custom(600, "Global.NotInitialized", "Not initialized", metadata);

    public static Error InitializationFailed(Dictionary<string, object>? metadata = null) =>
        Error.Custom(601, "Global.InitializationFailed", "Initialization failed", metadata);
}
