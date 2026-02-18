using SpaceBattle.Core.Abstractions;

namespace SpaceBattle.Core.Commands.Decorators;

public sealed class ConditionalCommandDecorator : ICommand
{
    private readonly ICommand _innerCommand;
    private readonly IReadOnlyCollection<ICommandCondition> _conditions;

    public ConditionalCommandDecorator(ICommand innerCommand, params ICommandCondition[] conditions)
        : this(innerCommand, (IEnumerable<ICommandCondition>)conditions)
    {
    }

    public ConditionalCommandDecorator(ICommand innerCommand, IEnumerable<ICommandCondition> conditions)
    {
        _innerCommand = innerCommand ?? throw new ArgumentNullException(nameof(innerCommand));
        _conditions = conditions?.ToArray() ?? throw new ArgumentNullException(nameof(conditions));

        if (_conditions.Count == 0)
        {
            throw new ArgumentException("At least one condition must be provided.", nameof(conditions));
        }
    }

    public void Execute()
    {
        foreach (var condition in _conditions)
        {
            condition.EnsureSatisfied();
        }

        _innerCommand.Execute();
    }
}
