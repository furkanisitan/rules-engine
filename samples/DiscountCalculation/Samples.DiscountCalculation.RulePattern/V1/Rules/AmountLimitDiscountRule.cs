using RulesEngine;

namespace Samples.DiscountCalculation.RulePattern.V1.Rules;

[Rule(101)]
public class AmountLimitDiscountRule : IDiscountRule
{
    private const decimal DiscountAmountLimit = 10_000M;

    public bool CanApply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        return response is { DiscountAmount: > DiscountAmountLimit };
    }

    public DiscountRuleResponse Apply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        response.SetDiscountAmount(DiscountAmountLimit, request.Amount);
        return response;
    }
}