namespace RuleDesignPattern.RuleEngine;

public enum RuleType
{
    /// <summary>
    ///     The <see cref="RuleExecutor.ExecuteAll{TRule,TRequest,TResponse}(TRequest)" /> method
    ///     does not execute rules of this rule type.
    /// </summary>
    None,

    /// <summary>
    ///     Rules of this type are only executed
    ///     in the <see cref="RuleExecutor.ExecuteAll{TRule,TRequest,TResponse}(TRequest)" /> method.
    /// </summary>
    Independent,

    /// <summary>
    ///     Rules of this type are executed at the end of all rule execution methods by default.
    /// </summary>
    Finish
}