using RuleDesignPattern.RuleExecutor.Rules;

namespace RuleDesignPattern.RuleExecutor.Tests.DiscountCalculation;

internal interface IDiscountRule : IRule<DiscountRuleRequest, DiscountRuleResponse>
{
}