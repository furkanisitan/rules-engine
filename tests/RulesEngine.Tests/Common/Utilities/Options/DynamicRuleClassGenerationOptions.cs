namespace RulesEngine.Tests.Common.Utilities.Options;

public record DynamicRuleClassGenerationOptions(string ClassName)
{
    public RuleAttribute? RuleAttribute { get; set; }
}