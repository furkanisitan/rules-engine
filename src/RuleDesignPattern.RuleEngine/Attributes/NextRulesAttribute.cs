namespace RuleDesignPattern.RuleEngine.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class NextRulesAttribute : Attribute
{
    public Type[] Rules;

    public NextRulesAttribute(params Type[] rules)
    {
        Rules = rules;
    }
}