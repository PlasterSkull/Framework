namespace PlasterSkull.Framework.Blazor;

public static class AppKindExt
{
    public static bool IsServer(this AppKind appKind) =>
        appKind is AppKind.WebServer;

    public static bool IsClient(this AppKind appKind) =>
        appKind is AppKind.WasmApp or AppKind.MauiApp;
    public static bool IsWasmApp(this AppKind appKind) =>
        appKind is AppKind.WasmApp;
    public static bool IsMauiApp(this AppKind appKind) =>
        appKind is AppKind.MauiApp;

    public static bool HasBlazorUI(this AppKind appKind) =>
        appKind is not AppKind.Unknown;
}
