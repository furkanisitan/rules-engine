namespace RuleDesignPattern.RuleExecutor.Tests.DiscountCalculation;

internal class DiscountRuleResponse : IRuleResponse
{
    public decimal TotalDiscountAmount { get; set; }
    public decimal TotalDiscountRate { get; set; }
}