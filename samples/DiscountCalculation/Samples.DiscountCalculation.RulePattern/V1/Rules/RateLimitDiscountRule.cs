using RulesEngine;

namespace Samples.DiscountCalculation.RulePattern.V1.Rules;

[Rule(100)]
public class RateLimitDiscountRule : IDiscountRule
{
    private const decimal DiscountRateLimit = .4M;

    public bool CanApply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        return response is { DiscountRate: > DiscountRateLimit };
    }

    public DiscountRuleResponse Apply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        response.SetDiscountRate(DiscountRateLimit, request.Amount);
        return response;
    }
}