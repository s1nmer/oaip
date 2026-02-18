using SpaceBattle.Core.Abstractions;

namespace SpaceBattle.Core.Commands;

public sealed class FireCommand : ICommand
{
    private readonly IFireable _fireable;

    public FireCommand(IFireable fireable)
    {
        _fireable = fireable ?? throw new ArgumentNullException(nameof(fireable));
    }

    public void Execute()
    {
        _fireable.Fire();
    }
}
