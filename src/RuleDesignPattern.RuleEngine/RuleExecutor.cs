using System.Reflection;
using RuleDesignPattern.Core.Extensions;
using RuleDesignPattern.RuleEngine.Attributes;

namespace RuleDesignPattern.RuleEngine;

public static class RuleExecutor
{
    public static TResponse ExecuteIndependents<TRule, TRequest, TResponse>(TRequest request)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return ExecuteIndependents<TRule, TRequest, TResponse>(request, new TResponse(), Assembly.GetCallingAssembly());
    }

    public static TResponse ExecuteIndependents<TRule, TRequest, TResponse>(
        TRequest request, params Assembly[] assemblies
    )
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return ExecuteIndependents<TRule, TRequest, TResponse>(request, new TResponse(), assemblies);
    }

    public static TResponse ExecuteIndependents<TRule, TRequest, TResponse>(TRequest request, TResponse response)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        return ExecuteIndependents<TRule, TRequest, TResponse>(request, response, Assembly.GetCallingAssembly());
    }

    public static TResponse ExecuteIndependents<TRule, TRequest, TResponse>(
        TRequest request, TResponse response, params Assembly[] assemblies
    )
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        var ruleType = typeof(TRule);
        var attrType = typeof(IndependentRuleAttribute);

        var concreteRuleTypes = ruleType
            .GetConcretes(assemblies)
            .Select(x => new
            {
                RuleType = x,
                (Attribute.GetCustomAttribute(x, attrType) as IndependentRuleAttribute)?.RunOrder
            })
            .Where(x => x.RunOrder is not null)
            .OrderBy(x => x.RunOrder)
            .Select(x => x.RuleType);

        return ExecuteByRuleTypes(request, response, concreteRuleTypes);
    }

    public static TResponse Execute<TRule, TRequest, TResponse>(TRule rule, TRequest request)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return Execute(rule, request, new TResponse());
    }

    public static TResponse Execute<TRule, TRequest, TResponse>(TRule rule, TRequest request, TResponse response)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        ArgumentNullException.ThrowIfNull(rule);
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(response);

        if (!rule.CanApply(request, response)) return response;
        response = rule.Apply(request, response);

        return response.StopRuleExecution ? response : ExecuteByRuleTypes(request, response, rule.NextRules);
    }

    private static TResponse ExecuteByRuleTypes<TRequest, TResponse>(
        TRequest request, TResponse response, IEnumerable<Type> ruleTypes
    )
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        foreach (var ruleType in ruleTypes)
        {
            if (Activator.CreateInstance(ruleType) is not IRule<TRequest, TResponse> rule) continue;
            response = Execute(rule, request, response);
        }

        return response;
    }
}