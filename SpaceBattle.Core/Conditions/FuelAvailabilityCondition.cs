using SpaceBattle.Core.Abstractions;
using SpaceBattle.Core.Exceptions;
using SpaceBattle.Core.GameObjects;

namespace SpaceBattle.Core.Conditions;

public sealed class FuelAvailabilityCondition : ICommandCondition
{
    private readonly UObject _uObject;
    private readonly float _fuelCost;
    private readonly string _fuelKey;

    public FuelAvailabilityCondition(UObject uObject, float fuelCost, string fuelKey = PropertyKeys.FuelAmount)
    {
        if (fuelCost < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fuelCost), "Fuel cost cannot be negative.");
        }

        _uObject = uObject ?? throw new ArgumentNullException(nameof(uObject));
        _fuelCost = fuelCost;
        _fuelKey = fuelKey;
    }

    public void EnsureSatisfied()
    {
        var currentFuel = _uObject.Get<float>(_fuelKey);
        if (currentFuel < _fuelCost)
        {
            throw new ResourceUnavailableException("Fuel", $"Required {_fuelCost}, available {currentFuel}.");
        }

        _uObject.Set(_fuelKey, currentFuel - _fuelCost);
    }
}
