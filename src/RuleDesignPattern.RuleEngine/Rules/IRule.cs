namespace RuleDesignPattern.RuleEngine.Rules;

public interface IRule<in TRequest, in TResponse>
    where TRequest : IRuleRequest
    where TResponse : IRuleResponse, new()
{
    bool CanApply(TRequest request);

    void Apply(TRequest request, TResponse response);
}