using RuleDesignPattern.Samples.DiscountCalculation.V1;
using RuleDesignPattern.Samples.DiscountCalculation.V1.RuleDesign;

namespace RuleDesignPattern.Samples.DiscountCalculation.Tests.V1;

[TestFixture]
internal class DiscountCalculatorTests
{
    [TestCaseSource(nameof(TestData))]
    public void CalculateDiscount_InputIsFromTestData_ReturnValueIsEqualExpectedDiscountAmount(
        DiscountCalculationRequest request, decimal expectedDiscountAmount
    )
    {
        var discountAmount = DiscountCalculator.CalculateDiscount(request);

        Assert.AreEqual(expectedDiscountAmount, discountAmount);
    }

    [TestCaseSource(nameof(TestData))]
    public void CalculateDiscountWithRules_InputIsFromTestData_ReturnValueIsEqualExpectedDiscountAmount(
        DiscountCalculationRequest request, decimal expectedDiscountAmount
    )
    {
        var discountRuleRequest = new DiscountRuleRequest
        {
            Amount = request.Amount,
            IsCitizen = request.IsCitizen,
            IsStudent = request.IsStudent,
            IsMarried = request.IsMarried,
            IsVictim = request.IsVictim
        };

        var discountAmount = DiscountCalculator.CalculateDiscountWithRules(discountRuleRequest);

        Assert.AreEqual(expectedDiscountAmount, discountAmount);
    }

    private static readonly object[] TestData =
    {
        // Rate Limit
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 10_000M, IsVictim = false, IsCitizen = true, IsStudent = true, IsMarried = true },
            4_000M
        },
        // Rate Limit, Amount Limit
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = false, IsCitizen = true, IsStudent = true, IsMarried = true },
            10_000M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = false, IsCitizen = true, IsStudent = true, IsMarried = false },
            9_000M
        },
        // Amount Limit
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = false, IsCitizen = true, IsStudent = false, IsMarried = true },
            10_000M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = false, IsCitizen = true, IsStudent = false, IsMarried = false },
            0M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = false, IsCitizen = false, IsStudent = true, IsMarried = true },
            0M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = false, IsCitizen = false, IsStudent = true, IsMarried = false },
            0M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = false, IsCitizen = false, IsStudent = false, IsMarried = true },
            0M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = false, IsCitizen = false, IsStudent = false, IsMarried = false },
            0M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = true, IsCitizen = true, IsStudent = true, IsMarried = true },
            22_500M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = true, IsCitizen = true, IsStudent = true, IsMarried = false },
            22_500M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = true, IsCitizen = true, IsStudent = false, IsMarried = true },
            22_500M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = true, IsCitizen = true, IsStudent = false, IsMarried = false },
            22_500M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = true, IsCitizen = false, IsStudent = true, IsMarried = true },
            22_500M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = true, IsCitizen = false, IsStudent = true, IsMarried = false },
            22_500M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = true, IsCitizen = false, IsStudent = false, IsMarried = true },
            22_500M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = true, IsCitizen = false, IsStudent = false, IsMarried = false },
            22_500M
        }
    };
}