namespace RuleDesignPattern.RuleEngine;

/// <summary>
///     Represents the base implementation of a rule.
/// </summary>
public interface IRule<in TRequest, TResponse>
    where TRequest : IRuleRequest
    where TResponse : IRuleResponse
{
    IEnumerable<Type> NextRules =>
        Attribute.GetCustomAttribute(GetType(), typeof(RuleAttribute)) is RuleAttribute attribute
            ? attribute.NextRules
            : Enumerable.Empty<Type>();

    bool CanApply(TRequest request, TResponse response);

    TResponse Apply(TRequest request, TResponse response);
}