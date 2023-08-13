using System.Reflection;
using RuleDesignPattern.RuleEngine.Enums;
using RuleDesignPattern.RuleEngine.Models;
using RuleDesignPattern.RuleEngine.Rules;

namespace RuleDesignPattern.RuleEngine;

public static class RuleExecutor
{
    /// <summary>
    ///     Executes rules that implement the <typeparamref name="TRule" />
    ///     and any other rules that linked to those rules.
    ///     Searches for rules in the <see cref="Assembly" /> of the method that invoked the currently executing method.
    /// </summary>
    /// <inheritdoc cref="ExecuteUnLinked{TRule,TRequest,TResponse}(TRequest,  Assembly[])" />
    public static TResponse ExecuteUnLinked<TRule, TRequest, TResponse>(TRequest request)
        where TRule : IUnLinkedRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return ExecuteUnLinked<TRule, TRequest, TResponse>(request, Assembly.GetCallingAssembly());
    }

    /// <summary>
    ///     Executes rules that implement the <typeparamref name="TRule" />
    ///     and any other rules that linked to those rules.
    ///     Searches for rules in each <see cref="Assembly" /> of the <paramref name="assemblies" /> array.
    /// </summary>
    /// <typeparam name="TRule"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request">The input parameter of the rule.</param>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>The <typeparamref name="TResponse" /> object.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="request" /> is null.</exception>
    public static TResponse ExecuteUnLinked<TRule, TRequest, TResponse>(TRequest request, params Assembly[] assemblies)
        where TRule : IUnLinkedRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        var response = new TResponse();
        var ruleInfos = RuleHelper.GetConcreteRuleInfos(typeof(TRule), assemblies)
            .OrderByDescending(x => x.RuleOption?.RuleType ?? RuleType.None);

        foreach (var ruleInfo in ruleInfos)
        {
            response = Execute(ruleInfo.RuleType, request, response);
            if (ruleInfo.RuleOption is { RuleType: RuleType.StartBreak }) break;
        }

        return response;
    }

    /// <summary>
    ///     Executes the <typeparamref name="TRule" /> and other rules that linked on this rule.
    /// </summary>
    /// <inheritdoc cref="Execute{TRequest,TResponse}(Type,TRequest,TResponse)" />
    public static TResponse Execute<TRule, TRequest, TResponse>(TRequest request, TResponse response)
        where TRule : IRule<TRequest, TResponse>, new()
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        return Execute(typeof(TRule), request, response);
    }

    /// <summary>
    ///     Executes the rule of type <paramref name="ruleType" /> and other rules that linked on this rule.
    /// </summary>
    /// <remarks>
    ///     Rules are executed recursively.
    ///     If the linked rule also has a linked rule, that rule will also be executed.
    /// </remarks>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="ruleType">The type of rule to execute.</param>
    /// <param name="request">The input parameter of the rule.</param>
    /// <param name="response">The result parameter of the rule.</param>
    /// <returns>The <paramref name="response" /> object.</returns>
    /// <exception cref="ArgumentNullException">If one of the arguments is null.</exception>
    private static TResponse Execute<TRequest, TResponse>(Type ruleType, TRequest request, TResponse response)
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        ArgumentNullException.ThrowIfNull(ruleType);
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(response);

        if (Activator.CreateInstance(ruleType) is not IRule<TRequest, TResponse> rule) return response;

        if (!rule.CanApply(request, response)) return response;
        response = rule.Apply(request, response);

        return rule.Options is null
            ? response
            : rule.Options.LinkedRules.Aggregate(response,
                (current, linkedRule) => Execute(linkedRule, request, current));
    }
}