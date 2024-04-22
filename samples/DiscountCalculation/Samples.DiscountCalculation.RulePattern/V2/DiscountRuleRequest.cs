using RulesEngine;

namespace Samples.DiscountCalculation.RulePattern.V2;

public record DiscountRuleRequest(
    decimal Amount,
    bool IsCitizen,
    bool IsStudent,
    bool IsMarried,
    bool IsVictim,
    byte ChildCount
) : IRuleRequest;