namespace RuleDesignPattern.RuleEngine;

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