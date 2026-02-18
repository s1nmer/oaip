using SpaceBattle.Core.Abstractions;
using SpaceBattle.Core.GameObjects;

namespace SpaceBattle.Core.Adapters;

public sealed class RotatableAdapter : IRotatable
{
    private readonly UObject _uObject;
    private readonly string _orientationKey;
    private readonly string _angularVelocityKey;

    public RotatableAdapter(UObject uObject, string orientationKey = PropertyKeys.Orientation, string angularVelocityKey = PropertyKeys.AngularVelocity)
    {
        _uObject = uObject ?? throw new ArgumentNullException(nameof(uObject));
        _orientationKey = orientationKey;
        _angularVelocityKey = angularVelocityKey;
    }

    public float OrientationDegrees
    {
        get => _uObject.Get<float>(_orientationKey);
        set => _uObject.Set(_orientationKey, value);
    }

    public float AngularVelocityDegrees => _uObject.Get<float>(_angularVelocityKey);
}
