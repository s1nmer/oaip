using SpaceBattle.Core.GameObjects;

namespace SpaceBattle.Core.Abstractions;

public interface ICommandRule
{
    IEnumerable<ICommandCondition> CreateConditions(UObject uObject, ICommand command);
}
