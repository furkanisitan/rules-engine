using RuleDesignPattern.RuleEngine;
using RuleDesignPattern.Samples.DiscountCalculation.V2.RuleDesign;

namespace RuleDesignPattern.Samples.DiscountCalculation.V2;

public static class DiscountCalculator
{
    public static decimal CalculateDiscount(DiscountCalculationRequest request)
    {
        var discountAmount = 0M;

        #region Business Rules

        if (request.IsVictim)
            return request.Amount * .5M;

        if (request.IsStudent)
            discountAmount += request.Amount * .2M;

        if (request is { IsCitizen: true, IsMarried: true })
        {
            discountAmount += request.Amount * .25M;

            if (request.ChildCount > 0)
            {
                const byte maxChildCountForDiscount = 5;
                discountAmount += request.Amount * 0.02M * Math.Min(request.ChildCount, maxChildCountForDiscount);
            }
        }

        #endregion

        #region Limit Controls

        const decimal maxDiscountRate = .4M;
        const decimal maxDiscountAmount = 10_000M;

        var discountRate = discountAmount / request.Amount;
        if (discountRate > maxDiscountRate)
            discountAmount = request.Amount * maxDiscountRate;

        if (discountAmount > maxDiscountAmount)
            discountAmount = maxDiscountAmount;

        #endregion

        return discountAmount;
    }

    public static decimal CalculateDiscountWithRules(DiscountCalculationRequest request)
    {
        var discountRuleRequest = new DiscountRuleRequest
        {
            Amount = request.Amount,
            IsCitizen = request.IsCitizen,
            IsStudent = request.IsStudent,
            IsMarried = request.IsMarried,
            IsVictim = request.IsVictim,
            ChildCount = request.ChildCount
        };

        var response =
            RuleExecutor.ExecuteAll<IDiscountRule, DiscountRuleRequest, DiscountRuleResponse>(discountRuleRequest);

        return response.DiscountAmount;
    }
}