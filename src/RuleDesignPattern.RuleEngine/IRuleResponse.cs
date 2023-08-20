namespace RuleDesignPattern.RuleEngine;

/// <summary>
///     Represents a rule response dto for result parameters.
/// </summary>
public interface IRuleResponse
{
    /// <summary>
    ///     Specifies whether to stop rule execution.
    /// </summary>
    public bool StopRuleExecution { get; set; }
}