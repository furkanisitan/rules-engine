using System.Reflection;
using RuleDesignPattern.RuleEngine.Utils;

namespace RuleDesignPattern.RuleEngine;

public static class RuleExecutor
{
    /// <inheritdoc cref="ExecuteAll{TRule,TRequest,TResponse}(TRequest,TResponse,Assembly[])" />
    public static TResponse ExecuteAll<TRule, TRequest, TResponse>(TRequest request)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return ExecuteAll<TRule, TRequest, TResponse>(request, new TResponse(), Assembly.GetCallingAssembly());
    }

    /// <inheritdoc cref="ExecuteAll{TRule,TRequest,TResponse}(TRequest,TResponse,Assembly[])" />
    public static TResponse ExecuteAll<TRule, TRequest, TResponse>(TRequest request, params Assembly[] assemblies)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return ExecuteAll<TRule, TRequest, TResponse>(request, new TResponse(), assemblies);
    }

    /// <inheritdoc cref="ExecuteAll{TRule,TRequest,TResponse}(TRequest,TResponse,Assembly[])" />
    public static TResponse ExecuteAll<TRule, TRequest, TResponse>(TRequest request, TResponse response)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return ExecuteAll<TRule, TRequest, TResponse>(request, response, Assembly.GetCallingAssembly());
    }

    /// <summary>
    ///     Executes the <see cref="RuleType.Independent" /> and <see cref="RuleType.Finish" /> type concrete rules
    ///     that implement <typeparamref name="TRule" />.
    /// </summary>
    /// <remarks>
    ///     It also executes the rules in the <see cref="RuleAttribute.NextRules" /> property for each rule.
    /// </remarks>
    /// <typeparam name="TRule">The abstract type implemented by the concrete rules to be executed.</typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request">The object containing the rule input values.</param>
    /// <param name="response">The object containing the rule result values.</param>
    /// <param name="assemblies">
    ///     Assemblies to search for concrete rules.
    ///     The default is <see cref="Assembly.GetCallingAssembly()" />.
    /// </param>
    /// <returns>A <typeparamref name="TResponse" /> object containing the rule result values.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="request" /> or  <paramref name="response" /> is null.</exception>
    public static TResponse ExecuteAll<TRule, TRequest, TResponse>(
        TRequest request, TResponse response, params Assembly[] assemblies
    )
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        var concreteRules = RuleHelper.GetConcreteRules(assemblies, typeof(TRule),
            RuleType.Independent, RuleType.Finish);

        return ExecuteTypes(request, response, concreteRules);
    }

    /// <inheritdoc cref="Execute{TRule,TRequest,TResponse}(TRule,TRequest,TResponse)" />
    public static TResponse Execute<TRule, TRequest, TResponse>(TRule rule, TRequest request)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return Execute(rule, request, new TResponse(), Assembly.GetCallingAssembly());
    }

    /// <summary>
    ///     Executes the specified <paramref name="rule" /> and
    ///     other rules in the <see cref="RuleAttribute.NextRules" /> property.
    /// </summary>
    /// <typeparam name="TRule"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="rule">The concrete rule to be executed.</param>
    /// <param name="request">The object containing the rule input values.</param>
    /// <param name="response">The object containing the rule result values.</param>
    /// <returns>A <typeparamref name="TResponse" /> object containing the rule result values.</returns>
    public static TResponse Execute<TRule, TRequest, TResponse>(TRule rule, TRequest request, TResponse response)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        return Execute(rule, request, response, Assembly.GetCallingAssembly());
    }

    private static TResponse Execute<TRule, TRequest, TResponse>(
        TRule rule, TRequest request, TResponse response, params Assembly[] assemblies
    )
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        ArgumentNullException.ThrowIfNull(rule);

        var finishRules = RuleHelper.GetConcreteRules(assemblies, typeof(TRule), RuleType.Finish);
        var rules = new List<Type> { rule.GetType() }.Concat(finishRules);

        return ExecuteTypes(request, response, rules);
    }

    private static TResponse ExecuteTypes<TRequest, TResponse>(
        TRequest request, TResponse response, IEnumerable<Type> types
    )
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(response);

        foreach (var ruleType in types)
        {
            if (Activator.CreateInstance(ruleType) is not IRule<TRequest, TResponse> rule) continue;
            if (!rule.CanApply(request, response)) continue;

            response = rule.Apply(request, response);
            if (response.StopRuleExecution) return response;

            if (rule.NextRules.Any())
                response = ExecuteTypes(request, response, rule.NextRules);
        }

        return response;
    }
}