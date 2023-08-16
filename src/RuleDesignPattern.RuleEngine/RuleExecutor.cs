using RuleDesignPattern.RuleEngine.Utils;

namespace RuleDesignPattern.RuleEngine;

public static class RuleExecutor
{
    public static TResponse ExecuteAll<TRule, TRequest, TResponse>(TRequest request)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        return ExecuteAll<TRule, TRequest, TResponse>(request, new TResponse());
    }

    public static TResponse ExecuteAll<TRule, TRequest, TResponse>(TRequest request, TResponse response)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        var independentRules = RuleHelper.GetConcreteRules(typeof(TRule), RuleType.Independent);
        var finishRules = RuleHelper.GetConcreteRules(typeof(TRule), RuleType.Finish);
        var rules = independentRules.Concat(finishRules);

        return ExecuteTypes(request, response, rules);
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

        var finishRules = RuleHelper.GetConcreteRules(typeof(TRule), RuleType.Finish);
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
            if (!rule.CanApply(request, response)) return response;

            response = rule.Apply(request, response);
            if (response.StopRuleExecution) return response;

            response = ExecuteTypes(request, response, rule.NextRules);
        }

        return response;
    }
}