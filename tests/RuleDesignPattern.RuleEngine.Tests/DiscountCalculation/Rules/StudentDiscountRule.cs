using RuleDesignPattern.RuleEngine.Attributes;
using RuleDesignPattern.RuleEngine.Enums;

namespace RuleDesignPattern.RuleEngine.Tests.DiscountCalculation.Rules;

/// <summary>
///     Applies a 20% compound discount to citizens who are students.
/// </summary>
[RuleOption(RuleType.End)]
internal struct StudentDiscountRule : IDiscountRule
{
    private const decimal DiscountRate = 0.2M;

    public bool CanApply(DiscountRuleRequest request) =>
        request is { IsCitizen: true, IsStudent: true };

    public DiscountRuleResponse Apply(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        response.TotalDiscountRate += (1 - response.TotalDiscountRate) * DiscountRate;
        response.TotalDiscountAmount += (request.Price - response.TotalDiscountAmount) * DiscountRate;

        return response;
    }
}