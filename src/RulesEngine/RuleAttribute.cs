namespace RulesEngine;

/// <summary>
///     Specifies options for a rule.
///     This class cannot be inherited.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="RuleAttribute" /> class
///     with the specified <paramref name="executionOrder" />.
/// </remarks>
/// <param name="executionOrder">The order in which the rule will be executed.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class RuleAttribute(int executionOrder) : Attribute
{
    /// <summary>
    ///     Gets the order in which a rule will be executed.
    /// </summary>
    public int ExecutionOrder { get; } = executionOrder;

    /// <summary>
    ///     Gets or sets the parent rule to which a rule depends.
    /// </summary>
    /// <remarks>
    ///     If this property is set, this rule will not be executed until the parent rule is executed.
    /// </remarks>
    public Type? ParentRule { get; set; }
}