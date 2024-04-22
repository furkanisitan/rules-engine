using RulesEngine;

namespace Samples.DiscountCalculation.RulePattern.V2.Rules;

[Rule(1)]
public class VictimDiscountRule : IDiscountRule
{
    private const decimal Rate = .5M;

    public bool CanApply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        return request is { IsVictim: true };
    }

    public DiscountRuleResponse Apply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        response.SetDiscountRate(Rate, request.Amount);
        response.CanStopRulesExecution = true;
        return response;
    }
}