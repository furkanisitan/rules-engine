using RuleDesignPattern.RuleEngine.Enums;

namespace RuleDesignPattern.RuleEngine.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class RuleOptionAttribute : Attribute
{
    public RuleType RuleType;

    public RuleOptionAttribute(RuleType ruleType)
    {
        RuleType = ruleType;
    }
}