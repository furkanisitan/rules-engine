namespace RuleDesignPattern.RuleEngine.Rules;

public interface IRule<in TRequest, TResponse>
    where TRequest : IRuleRequest
    where TResponse : IRuleResponse, new()
{
    bool CanApply(TRequest request);

    TResponse Apply(TRequest request, TResponse response);
}