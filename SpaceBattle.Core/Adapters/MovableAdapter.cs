using System.Numerics;
using SpaceBattle.Core.Abstractions;
using SpaceBattle.Core.GameObjects;

namespace SpaceBattle.Core.Adapters;

public sealed class MovableAdapter : IMovable
{
    private readonly UObject _uObject;
    private readonly string _positionKey;
    private readonly string _velocityKey;

    public MovableAdapter(UObject uObject, string positionKey = PropertyKeys.Position, string velocityKey = PropertyKeys.Velocity)
    {
        _uObject = uObject ?? throw new ArgumentNullException(nameof(uObject));
        _positionKey = positionKey;
        _velocityKey = velocityKey;
    }

    public Vector2 Position
    {
        get => _uObject.Get<Vector2>(_positionKey);
        set => _uObject.Set(_positionKey, value);
    }

    public Vector2 Velocity => _uObject.Get<Vector2>(_velocityKey);
}
