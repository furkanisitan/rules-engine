using RuleDesignPattern.RuleEngine;

namespace RuleDesignPattern.Samples.DiscountCalculation.V1.RuleDesign.Rules.Limit;

[Rule(RuleType = RuleType.Finish, RunOrder = 1)]
internal class RateLimitDiscountRule : IDiscountRule
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