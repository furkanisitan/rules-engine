using RuleDesignPattern.RuleEngine;

namespace RuleDesignPattern.Samples.DiscountCalculation.V2.RuleDesign.Rules;

[Rule(typeof(ChildDiscountRule), RuleType = RuleType.Independent)]
internal class MarriedDiscountRule : IDiscountRule
{
    private const decimal Rate = 0.25M;

    public bool CanApply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        return request is { IsCitizen: true, IsMarried: true };
    }

    public DiscountRuleResponse Apply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        response.SetDiscountRate(response.DiscountRate + Rate, request.Amount);
        return response;
    }
}