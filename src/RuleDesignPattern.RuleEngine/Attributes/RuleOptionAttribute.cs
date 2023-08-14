using RuleDesignPattern.RuleEngine.Enums;

namespace RuleDesignPattern.RuleEngine.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class RuleOptionAttribute : Attribute
{
    public Type[] LinkedRules;
    public RuleType RuleType;

    public RuleOptionAttribute(params Type[] linkedRules)
    {
        RuleType = RuleType.None;
        LinkedRules = linkedRules;
    }

    public RuleOptionAttribute(RuleType ruleType, params Type[] linkedRules)
    {
        RuleType = ruleType;
        LinkedRules = linkedRules;
    }
}