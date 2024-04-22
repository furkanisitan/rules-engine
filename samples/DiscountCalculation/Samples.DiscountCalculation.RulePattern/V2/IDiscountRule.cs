using RulesEngine;

namespace Samples.DiscountCalculation.RulePattern.V2;

public interface IDiscountRule : IRule<DiscountRuleRequest, DiscountRuleResponse>;