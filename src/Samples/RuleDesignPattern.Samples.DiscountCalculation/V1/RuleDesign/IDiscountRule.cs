using RuleDesignPattern.RuleEngine;

namespace RuleDesignPattern.Samples.DiscountCalculation.V1.RuleDesign;

internal interface IDiscountRule : IRule<DiscountRuleRequest, DiscountRuleResponse>
{
}