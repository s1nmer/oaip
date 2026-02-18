using System.Collections.Concurrent;

namespace SpaceBattle.Core.GameObjects;

public sealed class UObject
{
    private readonly ConcurrentDictionary<string, object?> _properties = new(StringComparer.OrdinalIgnoreCase);

    public void Set<T>(string key, T value) => _properties[key] = value;

    public bool TryGet<T>(string key, out T value)
    {
        if (_properties.TryGetValue(key, out var stored) && stored is T typed)
        {
            value = typed;
            return true;
        }

        value = default!;
        return false;
    }

    public T Get<T>(string key)
    {
        if (!_properties.TryGetValue(key, out var stored))
        {
            throw new KeyNotFoundException($"Property '{key}' is not defined for the UObject.");
        }

        if (stored is not T typed)
        {
            throw new InvalidCastException($"Property '{key}' is not of type {typeof(T).FullName}.");
        }

        return typed;
    }

    public bool Contains(string key) => _properties.ContainsKey(key);
}
