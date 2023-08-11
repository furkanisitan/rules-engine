namespace RuleDesignPattern.RuleEngine.Tests.DiscountCalculation.Rules;

/// <summary>
///     Applies a 25% compound discount to married citizens.
/// </summary>
[RuleOption(RuleType.Start)]
internal struct MarriedDiscountRule : IDiscountRule
{
    private const decimal DiscountRate = 0.25M;

    public bool CanApply(DiscountRuleRequest request) =>
        request is { IsCitizen: true, IsMarried: true };

    public DiscountRuleResponse Apply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        response.TotalDiscountRate += (1 - response.TotalDiscountRate) * DiscountRate;
        response.TotalDiscountAmount += (request.Price - response.TotalDiscountAmount) * DiscountRate;

        return response;
    }
}