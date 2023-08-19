using RuleDesignPattern.RuleEngine;

namespace RuleDesignPattern.Samples.DiscountCalculation.V1.RuleDesign.Rules;

[Rule(RuleType = RuleType.Independent)]
internal class StudentDiscountRule : IDiscountRule
{
    private const decimal Rate = 0.2M;

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