namespace SpaceBattle.Core.Exceptions;

public sealed class ResourceUnavailableException : InvalidOperationException
{
    public ResourceUnavailableException(string resourceName, string? details = null)
        : base(details is null
            ? $"Resource '{resourceName}' is unavailable."
            : $"Resource '{resourceName}' is unavailable: {details}")
    {
        ResourceName = resourceName;
    }

    public string ResourceName { get; }
}
