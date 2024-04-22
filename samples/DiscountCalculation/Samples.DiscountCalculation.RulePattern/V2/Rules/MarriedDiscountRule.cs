using RulesEngine;

namespace Samples.DiscountCalculation.RulePattern.V2.Rules;

[Rule(3)]
public class MarriedDiscountRule : IDiscountRule
{
    private const decimal Rate = .25M;

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