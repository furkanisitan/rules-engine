using RuleDesignPattern.RuleEngine.Models;

namespace RuleDesignPattern.RuleEngine.Rules;

public interface IUnLinkedRule<in TRequest, TResponse> : IRule<TRequest, TResponse>
    where TRequest : IRuleRequest
    where TResponse : IRuleResponse
{
}