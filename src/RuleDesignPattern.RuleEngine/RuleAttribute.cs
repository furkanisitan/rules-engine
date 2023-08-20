namespace RuleDesignPattern.RuleEngine;

/// <summary>
///     Specifies the details of a <see cref="IRule{TRequest,TResponse}" /> class.
///     This class cannot be inherited.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class RuleAttribute : Attribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RuleAttribute" /> class
    ///     with the specified <paramref name="nextRules" />.
    /// </summary>
    /// <param name="nextRules">The types of rules that will be executed after this rule.</param>
    public RuleAttribute(params Type[] nextRules)
    {
        NextRules = nextRules;
        RuleType = RuleType.None;
    }

    /// <summary>
    ///     The types of rules that will be executed after this rule.
    /// </summary>
    public Type[] NextRules { get; }

    /// <summary>
    ///     Specifies the rule type.
    ///     The default is <see cref="RuleEngine.RuleType.None" />.
    /// </summary>
    /// <remarks>
    ///     The rule type affects the order in which the rules are executed.
    ///     The lower value rule type is executed first.
    /// </remarks>
    public RuleType RuleType { get; set; }

    /// <summary>
    ///     Specifies the order in which this rule will be executed.
    ///     The default is <see langword="0" />.
    /// </summary>
    public int RunOrder { get; set; }
}