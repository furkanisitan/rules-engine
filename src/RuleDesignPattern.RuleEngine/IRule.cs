using RuleDesignPattern.RuleEngine.Attributes;

namespace RuleDesignPattern.RuleEngine;

public interface IRule<in TRequest, TResponse>
    where TRequest : IRuleRequest
    where TResponse : IRuleResponse
{
    Type[] NextRules =>
        Attribute.GetCustomAttribute(GetType(), typeof(NextRulesAttribute)) is NextRulesAttribute attribute
            ? attribute.Rules
            : Enumerable.Empty<Type>().ToArray();

    bool CanApply(TRequest request, TResponse response);

    TResponse Apply(TRequest request, TResponse response);
}