using RuleDesignPattern.RuleEngine.Models;

namespace RuleDesignPattern.RuleEngine.Tests.DiscountCalculation;

internal struct DiscountRuleResponse : IRuleResponse
{
    public decimal TotalDiscountAmount { get; set; }
    public decimal TotalDiscountRate { get; set; }
}