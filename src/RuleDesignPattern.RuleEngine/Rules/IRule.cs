using RuleDesignPattern.RuleEngine.Models;

namespace RuleDesignPattern.RuleEngine.Rules;

public interface IRule<in TRequest, TResponse>
    where TRequest : IRuleRequest
    where TResponse : IRuleResponse
{
    bool CanApply(TRequest request, TResponse response);

    TResponse Apply(TRequest request, TResponse response);
}