namespace RuleDesignPattern.RuleEngine;

public static class RuleExecutor
{
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

        foreach (var nextRuleType in rule.NextRules)
        {
            if (Activator.CreateInstance(nextRuleType) is not IRule<TRequest, TResponse> nextRule) continue;
            response = Execute(nextRule, request, response);
        }

        return response;
    }
}