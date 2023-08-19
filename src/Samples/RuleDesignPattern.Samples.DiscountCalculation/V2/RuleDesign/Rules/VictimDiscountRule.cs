using RuleDesignPattern.RuleEngine;

namespace RuleDesignPattern.Samples.DiscountCalculation.V2.RuleDesign.Rules;

[Rule(RuleType = RuleType.Independent, RunOrder = int.MinValue)]
internal class VictimDiscountRule : IDiscountRule
{
    private const decimal Rate = 0.5M;

    public bool CanApply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        return request is { IsVictim: true };
    }

    public DiscountRuleResponse Apply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        response.SetDiscountRate(Rate, request.Amount);
        response.StopRuleExecution = true;
        return response;
    }
}