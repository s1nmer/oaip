using SpaceBattle.Core.GameObjects;

namespace SpaceBattle.Core.Abstractions;

public interface IGameRuleProvider
{
    IEnumerable<ICommandRule> GetRules(UObject uObject);
}
