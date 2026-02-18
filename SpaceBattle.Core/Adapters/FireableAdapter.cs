using SpaceBattle.Core.Abstractions;
using SpaceBattle.Core.GameObjects;

namespace SpaceBattle.Core.Adapters;

public sealed class FireableAdapter : IFireable
{
    private readonly UObject _uObject;
    private readonly string _missileCountKey;
    private readonly string _fireActionKey;

    public FireableAdapter(UObject uObject, string missileCountKey = PropertyKeys.MissileCount, string fireActionKey = PropertyKeys.FireAction)
    {
        _uObject = uObject ?? throw new ArgumentNullException(nameof(uObject));
        _missileCountKey = missileCountKey;
        _fireActionKey = fireActionKey;
    }

    public int MissileCount
    {
        get => _uObject.Get<int>(_missileCountKey);
        set => _uObject.Set(_missileCountKey, value);
    }

    public void Fire()
    {
        if (!_uObject.TryGet<Action>(_fireActionKey, out var fireAction))
        {
            throw new InvalidOperationException($"UObject does not define a fire action under key '{_fireActionKey}'.");
        }

        fireAction();
    }
}
