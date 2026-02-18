using System.Numerics;
using SpaceBattle.Core.Adapters;
using SpaceBattle.Core.Commands;
using SpaceBattle.Core.Commands.Decorators;
using SpaceBattle.Core.Exceptions;
using SpaceBattle.Core.GameObjects;
using SpaceBattle.Core.Rules;

var ship = new UObject();

ship.Set(PropertyKeys.Position, new Vector2(0, 0));
ship.Set(PropertyKeys.Velocity, new Vector2(2, 1));
ship.Set(PropertyKeys.Orientation, 0f);
ship.Set(PropertyKeys.AngularVelocity, 15f);
ship.Set(PropertyKeys.FuelAmount, 3f);
ship.Set(PropertyKeys.MissileCount, 2);
ship.Set(PropertyKeys.FireAction, () => Console.WriteLine("BOOM! Missile launched."));
ship.Set(PropertyKeys.RuleSubscriptions, new List<string> { "ship" });

var rules = new GameRuleProvider(
    new GameRuleProvider.RuleSubscription(
        "ship",
        new SpaceBattle.Core.Abstractions.ICommandRule[]
        {
            new FuelConsumptionRule(defaultFuelCost: 1f),
            new MissileConsumptionRule(typeof(FireCommand))
        }));

var movable = new MovableAdapter(ship);
var rotatable = new RotatableAdapter(ship);
var fireable = new FireableAdapter(ship);

var move = new MoveCommand(movable);
var rotate = new RotateCommand(rotatable);
var fire = new FireCommand(fireable);

ExecuteWithRules("Move", ship, move, rules);
ExecuteWithRules("Rotate", ship, rotate, rules);
ExecuteWithRules("Fire", ship, fire, rules);
ExecuteWithRules("Fire", ship, fire, rules);
ExecuteWithRules("Fire", ship, fire, rules);

Console.WriteLine();
Console.WriteLine($"Position: {ship.Get<Vector2>(PropertyKeys.Position)}");
Console.WriteLine($"Orientation: {ship.Get<float>(PropertyKeys.Orientation)} degrees");
Console.WriteLine($"Fuel: {ship.Get<float>(PropertyKeys.FuelAmount)}");
Console.WriteLine($"Missiles: {ship.Get<int>(PropertyKeys.MissileCount)}");

static void ExecuteWithRules(string name, UObject uObject, SpaceBattle.Core.Abstractions.ICommand command, SpaceBattle.Core.Abstractions.IGameRuleProvider ruleProvider)
{
    try
    {
        var conditions = ruleProvider
            .GetRules(uObject)
            .SelectMany(rule => rule.CreateConditions(uObject, command))
            .ToArray();

        if (conditions.Length == 0)
        {
            command.Execute();
        }
        else
        {
            var guarded = new ConditionalCommandDecorator(command, conditions);
            guarded.Execute();
        }

        Console.WriteLine($"{name}: OK");
    }
    catch (ResourceUnavailableException ex)
    {
        Console.WriteLine($"{name}: FAIL ({ex.ResourceName}) - {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{name}: ERROR - {ex.Message}");
    }
}
