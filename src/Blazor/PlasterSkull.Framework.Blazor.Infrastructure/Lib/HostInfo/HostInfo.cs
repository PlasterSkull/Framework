using Microsoft.Extensions.Hosting;

namespace PlasterSkull.Framework.Blazor;

public sealed record HostInfo
{
    private string _baseUrl = "";

    public required AppKind AppKind { get; init; }
    public required ClientKind ClientKind { get; init; }
    public required string Environment { get; init; } = Environments.Development;
    public string DeviceModel { get; init; } = "Unknown";
    public required string BaseUrl
    {
        get
        {
            if (_baseUrl.IsNullOrEmpty())
                throw new InvalidOperationException("BaseUrl are unspecified.");

            return _baseUrl;
        }
        init => _baseUrl = value;
    }

    public bool IsDevelopment => Environment == Environments.Development;
    public bool IsStaging => Environment == Environments.Staging;
    public bool IsProduction => Environment == Environments.Production;
}
