namespace RuleDesignPattern.RuleEngine;

public enum RuleType
{
    /// <summary>
    ///     It stops executing the rules if applied.
    ///     It is run before all rules.
    /// </summary>
    StartBreak,

    /// <summary>
    ///     The rule to run at startup.
    /// </summary>
    Start,

    /// <summary>
    ///     Default rule type.
    /// </summary>
    None,

    /// <summary>
    ///     The rule to run at the end.
    /// </summary>
    End
}