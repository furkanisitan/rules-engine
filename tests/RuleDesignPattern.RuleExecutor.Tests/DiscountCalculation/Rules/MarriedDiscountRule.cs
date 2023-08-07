namespace RuleDesignPattern.RuleExecutor.Tests.DiscountCalculation.Rules;

/// <summary>
///     Applies a 25% discount to married citizens.
/// </summary>
internal class MarriedDiscountRule : IDiscountRule
{
    public bool CanApply(DiscountRuleRequest request) =>
        request is { IsCitizen: true, IsMarried: true };

    public void Apply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        const decimal discountRate = 0.25M;

        response.TotalDiscountRate += discountRate;
        response.TotalDiscountAmount += request.Price * discountRate;
    }
}