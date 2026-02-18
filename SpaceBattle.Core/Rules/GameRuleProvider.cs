using SpaceBattle.Core.Abstractions;
using SpaceBattle.Core.GameObjects;

namespace SpaceBattle.Core.Rules;

public sealed class GameRuleProvider : IGameRuleProvider
{
    private readonly IReadOnlyDictionary<string, IReadOnlyCollection<ICommandRule>> _ruleSets;

    public GameRuleProvider(params RuleSubscription[] ruleSubscriptions)
        : this((IEnumerable<RuleSubscription>)ruleSubscriptions)
    {
    }

    public GameRuleProvider(IEnumerable<RuleSubscription> ruleSubscriptions)
    {
        if (ruleSubscriptions is null)
        {
            throw new ArgumentNullException(nameof(ruleSubscriptions));
        }

        _ruleSets = ruleSubscriptions
            .Select(subscription => subscription ?? throw new ArgumentException("Rule subscriptions cannot contain null entries.", nameof(ruleSubscriptions)))
            .GroupBy(subscription => subscription.Id, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyCollection<ICommandRule>)group
                    .SelectMany(subscription => subscription.Rules)
                    .Where(rule => rule is not null)
                    .ToArray(),
                StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<ICommandRule> GetRules(UObject uObject)
    {
        if (uObject is null)
        {
            throw new ArgumentNullException(nameof(uObject));
        }

        if (!uObject.TryGet<IReadOnlyCollection<string>>(PropertyKeys.RuleSubscriptions, out var subscriptionIds) || subscriptionIds.Count == 0)
        {
            return Array.Empty<ICommandRule>();
        }

        var rules = new List<ICommandRule>();

        foreach (var subscriptionId in subscriptionIds)
        {
            if (subscriptionId is null)
            {
                continue;
            }

            if (_ruleSets.TryGetValue(subscriptionId, out var ruleSet))
            {
                rules.AddRange(ruleSet);
            }
        }

        return rules;
    }

    public sealed record RuleSubscription(string Id, IReadOnlyCollection<ICommandRule> Rules)
    {
        public string Id { get; } = Id ?? throw new ArgumentNullException(nameof(Id));
        public IReadOnlyCollection<ICommandRule> Rules { get; } = Rules ?? throw new ArgumentNullException(nameof(Rules));
    }
}
