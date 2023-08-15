namespace RuleDesignPattern.RuleEngine;

public interface IRuleResponse
{
    public bool StopRuleExecution { get; set; }
}