using SpaceBattle.Core.Abstractions;
using SpaceBattle.Core.Exceptions;
using SpaceBattle.Core.GameObjects;

namespace SpaceBattle.Core.Conditions;

public sealed class MissileAvailabilityCondition : ICommandCondition
{
    private readonly UObject _uObject;
    private readonly string _missileCountKey;

    public MissileAvailabilityCondition(UObject uObject, string missileCountKey = PropertyKeys.MissileCount)
    {
        _uObject = uObject ?? throw new ArgumentNullException(nameof(uObject));
        _missileCountKey = missileCountKey;
    }

    public void EnsureSatisfied()
    {
        var missileCount = _uObject.Get<int>(_missileCountKey);
        if (missileCount <= 0)
        {
            throw new ResourceUnavailableException("Missiles", "No missiles remaining.");
        }

        _uObject.Set(_missileCountKey, missileCount - 1);
    }
}
