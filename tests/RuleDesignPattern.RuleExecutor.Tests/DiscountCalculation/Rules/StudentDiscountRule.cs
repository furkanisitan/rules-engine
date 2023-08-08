namespace RuleDesignPattern.RuleExecutor.Tests.DiscountCalculation.Rules;

/// <summary>
///     Applies a 20% compound discount to citizens who are students.
/// </summary>
[RuleOption(RuleType.End)]
internal class StudentDiscountRule : IDiscountRule
{
    public bool CanApply(DiscountRuleRequest request) =>
        request is { IsCitizen: true, IsStudent: true };

    public void Apply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        const decimal discountRate = 0.2M;

        response.TotalDiscountRate += (1 - response.TotalDiscountRate) * discountRate;
        response.TotalDiscountAmount += (request.Price - response.TotalDiscountAmount) * discountRate;
    }
}