namespace PlasterSkull.Framework.Core;

public static class ClientKindExt
{
    public static bool IsMobile(this ClientKind clientKind) =>
        clientKind is ClientKind.iOS or ClientKind.Android;
}
