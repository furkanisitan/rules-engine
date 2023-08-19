using RuleDesignPattern.Samples.DiscountCalculation.V2;

namespace RuleDesignPattern.Samples.DiscountCalculation.Tests.V2;

[TestFixture]
internal class DiscountCalculatorTests
{
    [TestCaseSource(nameof(TestDataWithoutChild))]
    [TestCaseSource(nameof(TestDataWithChild))]
    public void CalculateDiscount_InputIsFromTestData_ReturnValueIsEqualExpectedDiscountAmount(
        DiscountCalculationRequest request, decimal expectedDiscountAmount
    )
    {
        var discountAmount = DiscountCalculator.CalculateDiscount(request);

        Assert.AreEqual(expectedDiscountAmount, discountAmount);
    }

    [TestCaseSource(nameof(TestDataWithoutChild))]
    [TestCaseSource(nameof(TestDataWithChild))]
    public void CalculateDiscountWithRules_InputIsFromTestData_ReturnValueIsEqualExpectedDiscountAmount(
        DiscountCalculationRequest request, decimal expectedDiscountAmount
    )
    {
        var discountAmount = DiscountCalculator.CalculateDiscountWithRules(request);

        Assert.AreEqual(expectedDiscountAmount, discountAmount);
    }

    private static readonly object[] TestDataWithoutChild =
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
            9_000M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 45_000M, IsVictim = false, IsCitizen = false, IsStudent = true, IsMarried = false },
            9_000M
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

    private static readonly object[] TestDataWithChild =
    {
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 30_000M, IsCitizen = true, IsStudent = true, IsMarried = true, ChildCount = 5 },
            10_000M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 30_000M, IsCitizen = true, IsStudent = true, IsMarried = false, ChildCount = 5 },
            6_000M
        },

        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 30_000M, IsCitizen = true, IsStudent = false, IsMarried = true, ChildCount = 5 },
            10_000M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 30_000M, IsCitizen = true, IsStudent = false, IsMarried = true, ChildCount = 3 },
            9_300M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 10_000M, IsCitizen = true, IsStudent = false, IsMarried = true, ChildCount = 10 },
            3_500M
        },

        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 30_000M, IsCitizen = true, IsStudent = false, IsMarried = false, ChildCount = 5 },
            0M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 30_000M, IsCitizen = false, IsStudent = true, IsMarried = true, ChildCount = 5 },
            6_000M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 30_000M, IsCitizen = false, IsStudent = true, IsMarried = false, ChildCount = 5 },
            6_000M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 30_000M, IsCitizen = false, IsStudent = false, IsMarried = true, ChildCount = 5 },
            0M
        },
        new object[]
        {
            new DiscountCalculationRequest
                { Amount = 30_000M, IsCitizen = false, IsStudent = false, IsMarried = false, ChildCount = 5 },
            0M
        }
    };
}