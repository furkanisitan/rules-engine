using RulesEngine;

namespace Samples.DiscountCalculation.RulePattern.V2;

public static class DiscountCalculator
{
    public static decimal CalculateDiscount(DiscountRuleRequest request)
    {
        var response = RuleExecutor.Execute<IDiscountRule, DiscountRuleRequest, DiscountRuleResponse>(request);
        return response.DiscountAmount;
    }
}