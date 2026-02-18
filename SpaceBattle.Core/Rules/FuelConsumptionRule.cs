using SpaceBattle.Core.Abstractions;
using SpaceBattle.Core.Conditions;
using SpaceBattle.Core.GameObjects;

namespace SpaceBattle.Core.Rules;

public sealed class FuelConsumptionRule : ICommandRule
{
    private readonly float _defaultFuelCost;
    private readonly IReadOnlyDictionary<Type, float> _fuelCosts;

    public FuelConsumptionRule(float defaultFuelCost, IReadOnlyDictionary<Type, float>? fuelCosts = null)
    {
        if (defaultFuelCost < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(defaultFuelCost), "Fuel cost cannot be negative.");
        }

        _defaultFuelCost = defaultFuelCost;
        _fuelCosts = fuelCosts ?? new Dictionary<Type, float>();
    }

    public IEnumerable<ICommandCondition> CreateConditions(UObject uObject, ICommand command)
    {
        var commandType = command.GetType();
        var cost = _fuelCosts.TryGetValue(commandType, out var specificCost)
            ? specificCost
            : _defaultFuelCost;

        if (cost <= 0)
        {
            yield break;
        }

        yield return new FuelAvailabilityCondition(uObject, cost);
    }
}
