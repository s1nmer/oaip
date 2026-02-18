using SpaceBattle.Core.GameObjects;

namespace SpaceBattle.Core.Abstractions;

public interface ICommandExecutor
{
    void Execute(UObject uObject, ICommand command);
}
