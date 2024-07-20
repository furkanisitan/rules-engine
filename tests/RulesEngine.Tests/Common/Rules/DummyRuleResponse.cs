namespace RulesEngine.Tests.Common.Rules;

public class DummyRuleResponse : IRuleResponse
{
    public bool CanStopRulesExecution { get; set; }
}