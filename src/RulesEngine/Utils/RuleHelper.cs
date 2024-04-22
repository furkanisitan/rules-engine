using RulesEngine.Extensions;
using System.Reflection;

namespace RulesEngine.Utils;

internal static class RuleHelper
{
    /// <summary>
    ///     Searches main(not having a parent rule) rules that implement the <typeparamref name="TRule" />
    ///     in each <see cref="Assembly" /> of the <paramref name="assemblies" /> array
    ///     and have the <see cref="RuleAttribute" /> attribute.
    /// </summary>
    /// <typeparam name="TRule"></typeparam>
    /// <typeparam name="TRuleRequest"></typeparam>
    /// <typeparam name="TRuleResponse"></typeparam>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>
    ///     A collection of rule objects ordered by <see cref="RuleAttribute.ExecutionOrder" />.
    /// </returns>
    public static IEnumerable<TRule> GetMainRules<TRule, TRuleRequest, TRuleResponse>(Assembly[] assemblies)
        where TRule : IRule<TRuleRequest, TRuleResponse>
        where TRuleRequest : IRuleRequest
        where TRuleResponse : IRuleResponse
    {
        return GetRules<TRule, TRuleRequest, TRuleResponse>(assemblies, x => x.Attribute.ParentRule is null);
    }

    /// <summary>
    ///     Searches child rules that have the <see cref="RuleAttribute" /> attribute of the <paramref name="rule" />
    ///     in each <see cref="Assembly" /> of the <paramref name="assemblies" /> array.
    /// </summary>
    /// <typeparam name="TRule"></typeparam>
    /// <typeparam name="TRuleRequest"></typeparam>
    /// <typeparam name="TRuleResponse"></typeparam>
    /// <param name="rule">The parent rule whose child rules will be searched.</param>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>
    ///     A collection of rule objects ordered by <see cref="RuleAttribute.ExecutionOrder" />.
    /// </returns>
    public static IEnumerable<TRule> GetChildRulesOf<TRule, TRuleRequest, TRuleResponse>(TRule rule, Assembly[] assemblies)
        where TRule : IRule<TRuleRequest, TRuleResponse>
        where TRuleRequest : IRuleRequest
        where TRuleResponse : IRuleResponse
    {
        return GetRules<TRule, TRuleRequest, TRuleResponse>(assemblies, x => x.Attribute.ParentRule == rule.GetType());
    }

    private static IEnumerable<TRule> GetRules<TRule, TRuleRequest, TRuleResponse>(Assembly[] assemblies, Func<(Type Type, RuleAttribute Attribute), bool> filter)
        where TRule : IRule<TRuleRequest, TRuleResponse>
        where TRuleRequest : IRuleRequest
        where TRuleResponse : IRuleResponse
    {
        return typeof(TRule)
            .GetConcretesWithAttribute<RuleAttribute>(assemblies)
            .Where(filter)
            .Where(x => x.Type.HasParameterlessConstructor())
            .OrderBy(x => x.Attribute.ExecutionOrder)
            .Select(x => (TRule)Activator.CreateInstance(x.Type)!);
    }
}