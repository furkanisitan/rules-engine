using RulesEngine;

namespace Samples.DiscountCalculation.RulePattern.V1.Rules;

[Rule(2)]
public class StudentDiscountRule : IDiscountRule
{
    private const decimal Rate = .2M;

    public bool CanApply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        return request is { IsCitizen: true, IsStudent: true };
    }

    public DiscountRuleResponse Apply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        response.SetDiscountRate(response.DiscountRate + Rate, request.Amount);
        return response;
    }
}