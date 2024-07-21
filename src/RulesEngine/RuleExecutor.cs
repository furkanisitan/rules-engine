using RulesEngine.Utils;
using System.Reflection;

namespace RulesEngine;

public static class RuleExecutor
{
    /// <inheritdoc cref="Execute{TRule,TRequest,TResponse}(TRequest,TResponse,Assembly[])" />
    public static TResponse Execute<TRule, TRequest, TResponse>(TRequest request)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return Execute<TRule, TRequest, TResponse>(request, new TResponse(), Assembly.GetCallingAssembly());
    }

    /// <inheritdoc cref="Execute{TRule,TRequest,TResponse}(TRequest,TResponse,Assembly[])" />
    public static TResponse Execute<TRule, TRequest, TResponse>(TRequest request, params Assembly[] assemblies)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return Execute<TRule, TRequest, TResponse>(request, new TResponse(), assemblies);
    }

    /// <inheritdoc cref="Execute{TRule,TRequest,TResponse}(TRequest,TResponse,Assembly[])" />
    public static TResponse Execute<TRule, TRequest, TResponse>(TRequest request, TResponse response)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        return Execute<TRule, TRequest, TResponse>(request, response, Assembly.GetCallingAssembly());
    }

    /// <summary>
    ///     Executes main(not having a parent rule) rules that implement the <typeparamref name="TRule" />
    ///     in each <see cref="Assembly" /> of the <paramref name="assemblies" /> array
    ///     and have the <see cref="RuleAttribute" /> attribute.
    /// </summary>
    /// <typeparam name="TRule">The base abstract type of the concrete rules to be executed.</typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request">The object containing the rule input values.</param>
    /// <param name="response">The object containing the rule result values.</param>
    /// <param name="assemblies">
    ///     Assemblies to search for concrete rules.
    ///     The default is <see cref="Assembly.GetCallingAssembly()" />.
    /// </param>
    /// <returns>A <typeparamref name="TResponse" /> object containing the rule result values.</returns>
    /// <exception cref="ArgumentNullException">If one of the arguments is null.</exception>
    /// <exception cref="ArgumentException">If <paramref name="assemblies" /> is empty.</exception>
    public static TResponse Execute<TRule, TRequest, TResponse>(TRequest request, TResponse response, params Assembly[] assemblies)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(response);
        ArgumentNullException.ThrowIfNull(assemblies);
        if (assemblies.Length == 0)
            throw new ArgumentException($"'{nameof(assemblies)}' must contain at least one element.");

        var mainRules = RuleHelper.GetMainRules<TRule, TRequest, TResponse>(assemblies);
        return ExecuteRules(mainRules, request, response, assemblies);
    }


    private static TResponse ExecuteRules<TRule, TRequest, TResponse>(IEnumerable<TRule> rules, TRequest request, TResponse response, params Assembly[] assemblies)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        foreach (var rule in rules)
        {
            if (!rule.CanApply(request, response)) continue;

            response = rule.Apply(request, response);
            if (response.CanStopRulesExecution) return response;

            var childRules = RuleHelper.GetChildRulesOf<TRule, TRequest, TResponse>(rule, assemblies);
            response = ExecuteRules(childRules, request, response, assemblies);
            if (response.CanStopRulesExecution) return response;
        }

        return response;
    }
}