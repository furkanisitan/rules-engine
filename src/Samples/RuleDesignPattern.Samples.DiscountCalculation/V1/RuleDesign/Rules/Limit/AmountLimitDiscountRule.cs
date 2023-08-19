using RuleDesignPattern.RuleEngine;

namespace RuleDesignPattern.Samples.DiscountCalculation.V1.RuleDesign.Rules.Limit;

[Rule(RuleType = RuleType.Finish, RunOrder = 2)]
internal class AmountLimitDiscountRule : IDiscountRule
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