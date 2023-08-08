using System.Reflection;

namespace RuleDesignPattern.RuleExecutor;

public static class RuleExecutor
{
    public static TResponse Execute<TRule, TRequest, TResponse>(TRequest request, params Assembly[] assemblies)
        where TRule : IRule<TRequest, TResponse>
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse, new()
    {
        var response = new TResponse();

        var assemblyList = assemblies.ToList();
        assemblyList.Add(Assembly.GetCallingAssembly());

        var ruleTypes = assemblyList
            .Where(a => !a.IsDynamic)
            .Distinct()
            .SelectMany(a => a.DefinedTypes)
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsAssignableTo(typeof(TRule)))
            .ToList();

        foreach (var ruleType in ruleTypes)
        {
            if (Activator.CreateInstance(ruleType) is not IRule<TRequest, TResponse> rule) continue;
            if (!rule.CanApply(request)) continue;

            rule.Apply(request, response);

            if (Attribute.GetCustomAttribute(ruleType, typeof(RuleOptionAttribute)) is RuleOptionAttribute { RuleType: RuleType.StartBreak })
                break;
        }

        return response;
    }
}