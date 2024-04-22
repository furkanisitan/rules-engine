using RulesEngine;

namespace Samples.DiscountCalculation.RulePattern.V2.Rules;

[Rule(0, ParentRule = typeof(MarriedDiscountRule))]
public class ChildDiscountRule : IDiscountRule
{
    private const byte MaxChildCount = 5;
    private const decimal Rate = .02M;

    public bool CanApply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        return request is { ChildCount: > 0 };
    }

    public DiscountRuleResponse Apply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        var totalDiscountRate = Rate * Math.Min(request.ChildCount, MaxChildCount);
        response.SetDiscountRate(response.DiscountRate + totalDiscountRate, request.Amount);
        return response;
    }
}