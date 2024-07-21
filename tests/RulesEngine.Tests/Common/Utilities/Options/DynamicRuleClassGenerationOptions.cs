namespace RulesEngine.Tests.Common.Utilities.Options;

/// <summary>
///     Represents the options for generating a dynamic rule class.
/// </summary>
public record DynamicRuleClassGenerationOptions(string ClassName)
{
    public RuleAttribute? RuleAttribute { get; set; }
    public bool CanStopRulesExecution { get; set; }
}