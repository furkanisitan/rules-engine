using Samples.DiscountCalculation.RulePattern.V2;
using Samples.DiscountCalculation.Traditional.V2;
using DiscountCalculatorTraditional = Samples.DiscountCalculation.Traditional.V2.DiscountCalculator;
using DiscountCalculatorRulePattern = Samples.DiscountCalculation.RulePattern.V2.DiscountCalculator;

namespace Samples.DiscountCalculation.Tests.V2;

[TestFixture]
internal class DiscountCalculatorTests
{
    [TestCaseSource(nameof(TestData))]
    public void CalculateDiscount_TwoDifferentMethodsInvoked_ReturnValuesAreEqual(decimal amount, byte childCount, bool isCitizen, bool isStudent, bool isMarried, bool isVictim)
    {
        var traditionalRequest = new DiscountCalculationRequest(amount, isCitizen, isStudent, isMarried, isVictim, childCount);
        var rulePatternRequest = new DiscountRuleRequest(amount, isCitizen, isStudent, isMarried, isVictim, childCount);

        var responseTraditional = DiscountCalculatorTraditional.CalculateDiscount(traditionalRequest);
        var responseRulePattern = DiscountCalculatorRulePattern.CalculateDiscount(rulePatternRequest);

        Assert.That(responseRulePattern, Is.EqualTo(responseTraditional));
    }

    public static object[] TestData =
    [
        new object[] { 45_000M, (byte)0, true, true, true, true },
        new object[] { 45_000M, (byte)0, true, true, true, false },
        new object[] { 45_000M, (byte)0, true, true, false, true },
        new object[] { 45_000M, (byte)0, true, true, false, false },
        new object[] { 45_000M, (byte)0, true, false, true, true },
        new object[] { 45_000M, (byte)0, true, false, true, false },
        new object[] { 45_000M, (byte)0, true, false, false, true },
        new object[] { 45_000M, (byte)0, true, false, false, false },
        new object[] { 45_000M, (byte)0, false, true, true, true },
        new object[] { 45_000M, (byte)0, false, true, true, false },
        new object[] { 45_000M, (byte)0, false, true, false, true },
        new object[] { 45_000M, (byte)0, false, true, false, false },
        new object[] { 45_000M, (byte)0, false, false, true, true },
        new object[] { 45_000M, (byte)0, false, false, true, false },
        new object[] { 45_000M, (byte)0, false, false, false, true },
        new object[] { 45_000M, (byte)0, false, false, false, false },
        new object[] { 45_000M, (byte)3, true, true, true, true },
        new object[] { 45_000M, (byte)3, true, true, true, false },
        new object[] { 45_000M, (byte)3, true, true, false, true },
        new object[] { 45_000M, (byte)3, true, true, false, false },
        new object[] { 45_000M, (byte)3, true, false, true, true },
        new object[] { 45_000M, (byte)3, true, false, true, false },
        new object[] { 45_000M, (byte)3, true, false, false, true },
        new object[] { 45_000M, (byte)3, true, false, false, false },
        new object[] { 45_000M, (byte)3, false, true, true, true },
        new object[] { 45_000M, (byte)3, false, true, true, false },
        new object[] { 45_000M, (byte)3, false, true, false, true },
        new object[] { 45_000M, (byte)3, false, true, false, false },
        new object[] { 45_000M, (byte)3, false, false, true, true },
        new object[] { 45_000M, (byte)3, false, false, true, false },
        new object[] { 45_000M, (byte)3, false, false, false, true },
        new object[] { 45_000M, (byte)3, false, false, false, false },
        new object[] { 45_000M, (byte)5, true, true, true, true },
        new object[] { 45_000M, (byte)5, true, true, true, false },
        new object[] { 45_000M, (byte)5, true, true, false, true },
        new object[] { 45_000M, (byte)5, true, true, false, false },
        new object[] { 45_000M, (byte)5, true, false, true, true },
        new object[] { 45_000M, (byte)5, true, false, true, false },
        new object[] { 45_000M, (byte)5, true, false, false, true },
        new object[] { 45_000M, (byte)5, true, false, false, false },
        new object[] { 45_000M, (byte)5, false, true, true, true },
        new object[] { 45_000M, (byte)5, false, true, true, false },
        new object[] { 45_000M, (byte)5, false, true, false, true },
        new object[] { 45_000M, (byte)5, false, true, false, false },
        new object[] { 45_000M, (byte)5, false, false, true, true },
        new object[] { 45_000M, (byte)5, false, false, true, false },
        new object[] { 45_000M, (byte)5, false, false, false, true },
        new object[] { 45_000M, (byte)5, false, false, false, false },
        new object[] { 45_000M, (byte)8, true, true, true, true },
        new object[] { 45_000M, (byte)8, true, true, true, false },
        new object[] { 45_000M, (byte)8, true, true, false, true },
        new object[] { 45_000M, (byte)8, true, true, false, false },
        new object[] { 45_000M, (byte)8, true, false, true, true },
        new object[] { 45_000M, (byte)8, true, false, true, false },
        new object[] { 45_000M, (byte)8, true, false, false, true },
        new object[] { 45_000M, (byte)8, true, false, false, false },
        new object[] { 45_000M, (byte)8, false, true, true, true },
        new object[] { 45_000M, (byte)8, false, true, true, false },
        new object[] { 45_000M, (byte)8, false, true, false, true },
        new object[] { 45_000M, (byte)8, false, true, false, false },
        new object[] { 45_000M, (byte)8, false, false, true, true },
        new object[] { 45_000M, (byte)8, false, false, true, false },
        new object[] { 45_000M, (byte)8, false, false, false, true },
        new object[] { 45_000M, (byte)8, false, false, false, false }
    ];
}