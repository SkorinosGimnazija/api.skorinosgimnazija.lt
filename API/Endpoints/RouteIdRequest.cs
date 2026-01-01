namespace API.Endpoints;

using JetBrains.Annotations;

[PublicAPI]
public record RouteIdRequest<T>
{
    [RouteParam]
    public required T Id { get; init; }
}

[PublicAPI]
public record RouteIdRequest : RouteIdRequest<int>;

[PublicAPI]
public record RouteGuidRequest : RouteIdRequest<Guid>;