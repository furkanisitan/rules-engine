using RuleDesignPattern.RuleEngine.Rules;

namespace RuleDesignPattern.RuleEngine.Tests.DiscountCalculation;

internal interface IDiscountRule : IRule<DiscountRuleRequest, DiscountRuleResponse>
{
}