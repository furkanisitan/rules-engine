using System.Reflection;
using RuleDesignPattern.RuleEngine.Enums;
using RuleDesignPattern.RuleEngine.Models;
using RuleDesignPattern.RuleEngine.Rules;

namespace RuleDesignPattern.RuleEngine;

public static class RuleExecutor
{
    /// <remarks>
    ///     Searches for rules in the <see cref="Assembly" /> of the method that invoked the currently executing method.
    /// </remarks>
    /// <inheritdoc cref="ExecuteUnLinked{TRule,TRequest,TResponse}(TRequest,  Assembly[])" />
    public static TResponse ExecuteUnLinked<TRule, TRequest, TResponse>(TRequest request)
        where TRule : IUnLinkedRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return ExecuteUnLinked<TRule, TRequest, TResponse>(request, Assembly.GetCallingAssembly());
    }

    /// <summary>
    ///     Executes rules that implement the <typeparamref name="TRule" /> and any other rules that linked to these rules.
    /// </summary>
    /// <remarks>
    ///     Searches for rules in each <see cref="Assembly" /> of the <paramref name="assemblies" /> array.
    /// </remarks>
    /// <param name="request">The input parameter of the rule.</param>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>A <typeparamref name="TResponse" /> object.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="request" /> is null.</exception>
    public static TResponse ExecuteUnLinked<TRule, TRequest, TResponse>(TRequest request, params Assembly[] assemblies)
        where TRule : IUnLinkedRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return ExecuteByRuleInfos(request, new TResponse(), RuleHelper.GetConcreteRuleInfos(typeof(TRule), assemblies));
    }

    /// <summary>
    ///     Executes the rule of type <typeparamref name="TRule" /> and other rules that linked on this rule.
    /// </summary>
    /// <inheritdoc cref="ExecuteByRuleInfos{TRequest,TResponse}" />
    public static TResponse Execute<TRule, TRequest, TResponse>(TRequest request, TResponse response)
        where TRule : IRule<TRequest, TResponse>, new()
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        return ExecuteByRuleInfos(request, response, RuleHelper.GetRuleInfos(typeof(TRule)));
    }

    /// <summary>
    ///     Executes rules of type <see cref="RuleInfo.RuleType" /> in the <paramref name="ruleInfos" />
    ///     and other rules that linked to these rules.
    /// </summary>
    /// <param name="request">The input parameter of the rule.</param>
    /// <param name="response">The result parameter of the rule.</param>
    /// <param name="ruleInfos"><see cref="RuleInfo" /> objects containing rule types and other information.</param>
    /// <returns>The <paramref name="response" /> object.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="request" /> or <paramref name="response" /> is null.</exception>
    private static TResponse ExecuteByRuleInfos<TRequest, TResponse>(TRequest request, TResponse response,
        params RuleInfo[] ruleInfos)
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(response);

        foreach (var ruleInfo in ruleInfos.OrderBy(x => x.RuleOption?.RuleType ?? RuleType.None))
        {
            if (Activator.CreateInstance(ruleInfo.RuleType) is not IRule<TRequest, TResponse> rule) continue;
            if (!rule.CanApply(request, response)) continue;

            response = rule.Apply(request, response);

            if (ruleInfo.RuleOption is null) continue;

            response = ExecuteByRuleInfos(request, response,
                RuleHelper.GetRuleInfos(ruleInfo.RuleOption.LinkedRules).ToArray());

            if (ruleInfo.RuleOption is { RuleType: RuleType.StartBreak }) break;
        }

        return response;
    }
}