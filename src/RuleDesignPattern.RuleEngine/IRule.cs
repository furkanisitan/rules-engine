namespace RuleDesignPattern.RuleEngine;

/// <summary>
///     Represents the base implementation of a rule.
/// </summary>
public interface IRule<in TRequest, TResponse>
    where TRequest : IRuleRequest
    where TResponse : IRuleResponse
{
    /// <summary>
    ///     Returns <see cref="RuleAttribute.NextRules" /> property for this rule if exists,
    ///     otherwise an empty <see cref="IEnumerable{T}">IEnumerable&lt;<see cref="Type" />&gt;</see>.
    /// </summary>
    IEnumerable<Type> NextRules =>
        Attribute.GetCustomAttribute(GetType(), typeof(RuleAttribute)) is RuleAttribute attribute
            ? attribute.NextRules
            : Enumerable.Empty<Type>();


    /// <summary>
    ///     Determines whether this rule can be applied.
    /// </summary>
    /// <param name="request">The object containing the rule input values.</param>
    /// <param name="response">The object containing the rule result values.</param>
    /// <returns><see langword="true" />, if this rule is to be applied; otherwise,  <see langword="false" />.</returns>
    bool CanApply(TRequest request, TResponse response);

    /// <summary>
    ///     Applies rules on the <paramref name="response" /> object.
    /// </summary>
    /// <param name="request">The object containing the rule input values.</param>
    /// <param name="response">The object containing the rule result values.</param>
    /// <returns>A <typeparamref name="TResponse" /> object containing the rule result values.</returns>
    TResponse Apply(TRequest request, TResponse response);
}