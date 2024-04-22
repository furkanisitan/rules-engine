using RulesEngine;

namespace Samples.DiscountCalculation.RulePattern.V1;

public interface IDiscountRule : IRule<DiscountRuleRequest, DiscountRuleResponse>;