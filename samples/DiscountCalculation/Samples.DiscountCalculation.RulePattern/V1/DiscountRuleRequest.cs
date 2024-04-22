using RulesEngine;

namespace Samples.DiscountCalculation.RulePattern.V1;

public record DiscountRuleRequest(
    decimal Amount,
    bool IsCitizen,
    bool IsStudent,
    bool IsMarried,
    bool IsVictim
) : IRuleRequest;