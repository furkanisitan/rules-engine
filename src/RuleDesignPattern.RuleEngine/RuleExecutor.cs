using System.Reflection;
using RuleDesignPattern.RuleEngine.Attributes;
using RuleDesignPattern.RuleEngine.Enums;
using RuleDesignPattern.RuleEngine.Models;
using RuleDesignPattern.RuleEngine.Rules;

namespace RuleDesignPattern.RuleEngine;

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

        foreach (var ruleInfo in GetRuleInfos(typeof(TRule), assemblyList.ToArray()))
        {
            if (Activator.CreateInstance(ruleInfo.RuleType) is not IRule<TRequest, TResponse> rule) continue;
            if (!rule.CanApply(request, response)) continue;

            response = rule.Apply(request, response);
            if (ruleInfo.RuleOption is { RuleType: RuleType.StartBreak }) break;
        }

        return response;
    }

    public static IEnumerable<RuleInfo> GetRuleInfos(Type ruleType, params Assembly[] assemblies)
    {
        return assemblies
            .Where(a => !a.IsDynamic)
            .Distinct()
            .SelectMany(a => a.DefinedTypes)
            .Where(t => t is { IsAbstract: false } && t.IsAssignableTo(ruleType))
            .Select(t => new RuleInfo
            {
                RuleType = t,
                RuleOption = Attribute.GetCustomAttribute(t, typeof(RuleOptionAttribute)) as RuleOptionAttribute
            })
            .OrderByDescending(r => r.RuleOption?.RuleType ?? RuleType.None)
            .ToList();
    }

    /// <summary>
    ///     Executes the rule of type <typeparamref name="TRule" /> and other rules that linked on this type.
    /// </summary>
    /// <inheritdoc cref="Execute{TRequest,TResponse}" />
    public static TResponse Execute<TRule, TRequest, TResponse>(TRequest request, TResponse response)
        where TRule : IRule<TRequest, TResponse>, new()
        where TRequest : IRuleRequest
        where TResponse : IRuleResponse
    {
        return Execute(typeof(TRule), request, response);
    }

    /// <summary>
    ///     Executes the rule of type <paramref name="ruleType" /> and other rules that linked on this type.
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