namespace RuleDesignPattern.RuleEngine.Rules;

public interface IUnlinkedRule<in TRequest, TResponse> : IRule<TRequest, TResponse>
    where TRequest : IRuleRequest
    where TResponse : IRuleResponse, new()
{
}