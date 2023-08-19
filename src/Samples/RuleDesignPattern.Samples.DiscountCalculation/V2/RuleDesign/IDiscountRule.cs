using RuleDesignPattern.RuleEngine;

namespace RuleDesignPattern.Samples.DiscountCalculation.V2.RuleDesign;

internal interface IDiscountRule : IRule<DiscountRuleRequest, DiscountRuleResponse>
{
}