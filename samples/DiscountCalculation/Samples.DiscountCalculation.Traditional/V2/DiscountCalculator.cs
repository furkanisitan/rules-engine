namespace Samples.DiscountCalculation.Traditional.V2;

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
                discountAmount += request.Amount * .02M * Math.Min(request.ChildCount, maxChildCountForDiscount);
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
}