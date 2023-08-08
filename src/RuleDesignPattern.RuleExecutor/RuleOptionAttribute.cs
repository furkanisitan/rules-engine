namespace RuleDesignPattern.RuleExecutor;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class RuleOptionAttribute : Attribute
{
    public RuleType RuleType;

    public RuleOptionAttribute(RuleType ruleType)
    {
        RuleType = ruleType;
    }
}