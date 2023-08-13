using RuleDesignPattern.RuleEngine.Attributes;
using RuleDesignPattern.RuleEngine.Models;

namespace RuleDesignPattern.RuleEngine.Rules;

public interface IRule<in TRequest, TResponse>
    where TRequest : IRuleRequest
    where TResponse : IRuleResponse, new()
{
    RuleOptionAttribute? Options =>
        Attribute.GetCustomAttribute(GetType(), typeof(RuleOptionAttribute)) as RuleOptionAttribute;

    bool CanApply(TRequest request, TResponse response);

    TResponse Apply(TRequest request, TResponse response);
}