namespace RulesEngine;

/// <summary>
///     Represents the base implementation of a rule.
/// </summary>
public interface IRule<in TRequest, TResponse>
    where TRequest : IRuleRequest
    where TResponse : IRuleResponse
{
    /// <summary>
    ///     Determines whether the rule can be applied.
    /// </summary>
    /// <param name="request">The object containing the rule input values.</param>
    /// <param name="response">The object containing the rule result values.</param>
    /// <returns><see langword="true" />, if the rule is to be applied; otherwise, <see langword="false" />.</returns>
    bool CanApply(TRequest request, TResponse response);

    /// <summary>
    ///     Applies rules on the <paramref name="response" /> object.
    /// </summary>
    /// <param name="request">The object containing the rule input values.</param>
    /// <param name="response">The object containing the rule result values.</param>
    /// <returns>A <typeparamref name="TResponse" /> object representing the rule result values.</returns>
    TResponse Apply(TRequest request, TResponse response);
}