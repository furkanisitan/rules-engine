namespace RuleDesignPattern.RuleExecutor.Rules;

public interface ILinkedRule<in TRequest, in TResponse> : IRule<TRequest, TResponse>
    where TRequest : IRuleRequest
    where TResponse : IRuleResponse, new()
{
}