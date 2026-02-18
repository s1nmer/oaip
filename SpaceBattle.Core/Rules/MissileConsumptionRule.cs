using SpaceBattle.Core.Abstractions;
using SpaceBattle.Core.Conditions;
using SpaceBattle.Core.GameObjects;

namespace SpaceBattle.Core.Rules;

public sealed class MissileConsumptionRule : ICommandRule
{
    private readonly IReadOnlyCollection<Type> _affectedCommandTypes;

    public MissileConsumptionRule(params Type[] affectedCommandTypes)
        : this((IEnumerable<Type>)affectedCommandTypes)
    {
    }

    public MissileConsumptionRule(IEnumerable<Type> affectedCommandTypes)
    {
        if (affectedCommandTypes is null)
        {
            throw new ArgumentNullException(nameof(affectedCommandTypes));
        }

        var types = affectedCommandTypes.ToArray();
        if (types.Length == 0)
        {
            throw new ArgumentException("At least one command type must be provided.", nameof(affectedCommandTypes));
        }

        if (types.Any(t => !typeof(ICommand).IsAssignableFrom(t)))
        {
            throw new ArgumentException("All types must implement ICommand.", nameof(affectedCommandTypes));
        }

        _affectedCommandTypes = types;
    }

    public IEnumerable<ICommandCondition> CreateConditions(UObject uObject, ICommand command)
    {
        if (_affectedCommandTypes.Contains(command.GetType()))
        {
            yield return new MissileAvailabilityCondition(uObject);
        }
    }
}
