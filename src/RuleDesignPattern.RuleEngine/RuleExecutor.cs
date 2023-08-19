using System.Reflection;
using RuleDesignPattern.RuleEngine.Utils;

namespace RuleDesignPattern.RuleEngine;

public static class RuleExecutor
{
    public static TResponse ExecuteAll<TRule, TRequest, TResponse>(TRequest request)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return ExecuteAll<TRule, TRequest, TResponse>(request, new TResponse(), Assembly.GetCallingAssembly());
    }

    public static TResponse ExecuteAll<TRule, TRequest, TResponse>(TRequest request, params Assembly[] assemblies)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return ExecuteAll<TRule, TRequest, TResponse>(request, new TResponse(), assemblies);
    }

    public static TResponse ExecuteAll<TRule, TRequest, TResponse>(TRequest request, TResponse response)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return ExecuteAll<TRule, TRequest, TResponse>(request, response, Assembly.GetCallingAssembly());
    }

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

    public static TResponse Execute<TRule, TRequest, TResponse>(TRule rule, TRequest request)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return Execute(rule, request, new TResponse(), Assembly.GetCallingAssembly());
    }

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