namespace RuleDesignPattern.RuleEngine;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class RuleAttribute : Attribute
{
    public RuleAttribute(params Type[] nextRules)
    {
        NextRules = nextRules;
        RuleType = RuleType.None;
    }

    public Type[] NextRules { get; }
    public RuleType RuleType { get; set; }
    public int RunOrder { get; set; }
}