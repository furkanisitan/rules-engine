namespace RuleDesignPattern.RuleEngine.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class IndependentRuleAttribute : Attribute
{
    public int RunOrder { get; set; }
}