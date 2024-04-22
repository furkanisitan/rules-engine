using Samples.DiscountCalculation.RulePattern.V1;
using Samples.DiscountCalculation.Traditional.V1;
using DiscountCalculatorTraditional = Samples.DiscountCalculation.Traditional.V1.DiscountCalculator;
using DiscountCalculatorRulePattern = Samples.DiscountCalculation.RulePattern.V1.DiscountCalculator;

namespace Samples.DiscountCalculation.Tests.V1;

[TestFixture]
internal class DiscountCalculatorTests
{
    [TestCaseSource(nameof(TestData))]
    public void CalculateDiscount_TwoDifferentMethodsInvoked_ReturnValuesAreEqual(decimal amount, bool isCitizen, bool isStudent, bool isMarried, bool isVictim)
    {
        var traditionalRequest = new DiscountCalculationRequest(amount, isCitizen, isStudent, isMarried, isVictim);
        var rulePatternRequest = new DiscountRuleRequest(amount, isCitizen, isStudent, isMarried, isVictim);

        var responseTraditional = DiscountCalculatorTraditional.CalculateDiscount(traditionalRequest);
        var responseRulePattern = DiscountCalculatorRulePattern.CalculateDiscount(rulePatternRequest);

        Assert.That(responseRulePattern, Is.EqualTo(responseTraditional));
    }

    public static object[] TestData =
    [
        new object[] { 45_000M, true, true, true, true },
        new object[] { 45_000M, true, true, true, false },
        new object[] { 45_000M, true, true, false, true },
        new object[] { 45_000M, true, true, false, false },
        new object[] { 45_000M, true, false, true, true },
        new object[] { 45_000M, true, false, true, false },
        new object[] { 45_000M, true, false, false, true },
        new object[] { 45_000M, true, false, false, false },
        new object[] { 45_000M, false, true, true, true },
        new object[] { 45_000M, false, true, true, false },
        new object[] { 45_000M, false, true, false, true },
        new object[] { 45_000M, false, true, false, false },
        new object[] { 45_000M, false, false, true, true },
        new object[] { 45_000M, false, false, true, false },
        new object[] { 45_000M, false, false, false, true },
        new object[] { 45_000M, false, false, false, false }
    ];
}