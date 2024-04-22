namespace RulesEngine;

/// <summary>
///     Represents a rule response dto for result parameters.
/// </summary>
public interface IRuleResponse
{
    /// <summary>
    ///     Gets or sets whether rules execution can be stopped.
    /// </summary>
    public bool CanStopRulesExecution { get; set; }
}