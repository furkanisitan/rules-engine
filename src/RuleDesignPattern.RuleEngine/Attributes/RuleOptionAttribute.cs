using RuleDesignPattern.RuleEngine.Enums;

namespace RuleDesignPattern.RuleEngine.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class RuleOptionAttribute : Attribute
{
    public Type[] LinkedRules;
    public RuleType RuleType;

    public RuleOptionAttribute(RuleType ruleType, params Type[] linkedRules)
    {
        RuleType = ruleType;
        LinkedRules = linkedRules;
    }
}