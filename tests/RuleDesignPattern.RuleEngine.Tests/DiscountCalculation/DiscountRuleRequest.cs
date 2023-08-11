namespace RuleDesignPattern.RuleEngine.Tests.DiscountCalculation;

internal struct DiscountRuleRequest : IRuleRequest
{
    public decimal Price { get; set; }
    public bool IsCitizen { get; set; }
    public bool IsStudent { get; set; }
    public bool IsMarried { get; set; }
    public bool IsVictim { get; set; }
}