using SpaceBattle.Core.Abstractions;

namespace SpaceBattle.Core.Commands;

public sealed class RotateCommand : ICommand
{
    private readonly IRotatable _rotatable;

    public RotateCommand(IRotatable rotatable)
    {
        _rotatable = rotatable ?? throw new ArgumentNullException(nameof(rotatable));
    }

    public void Execute()
    {
        _rotatable.OrientationDegrees += _rotatable.AngularVelocityDegrees;
    }
}
