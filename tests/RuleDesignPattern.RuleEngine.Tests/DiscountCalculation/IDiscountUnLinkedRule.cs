using RuleDesignPattern.RuleEngine.Rules;

namespace RuleDesignPattern.RuleEngine.Tests.DiscountCalculation;

internal interface IDiscountUnLinkedRule : IUnLinkedRule<DiscountRuleRequest, DiscountRuleResponse>
{
}