using RuleDesignPattern.RuleExecutor.Tests.DiscountCalculation;

namespace RuleDesignPattern.RuleExecutor.Tests;

[TestFixture]
internal class RuleExecutor_ExecuteShould_DiscountCalculation
{
    public static object[] TestData =
    {
        new object[]
        {
            new DiscountRuleRequest { Price = 100, IsCitizen = true, IsStudent = true, IsMarried = true },
            new DiscountRuleResponse { TotalDiscountAmount = 40, TotalDiscountRate = .4M }
        },
        new object[]
        {
            new DiscountRuleRequest { Price = 100, IsCitizen = true, IsStudent = true, IsMarried = false },
            new DiscountRuleResponse { TotalDiscountAmount = 20, TotalDiscountRate = .2M }
        },
        new object[]
        {
            new DiscountRuleRequest { Price = 100, IsCitizen = true, IsStudent = false, IsMarried = true },
            new DiscountRuleResponse { TotalDiscountAmount = 25, TotalDiscountRate = .25M }
        },
        new object[]
        {
            new DiscountRuleRequest { Price = 100, IsCitizen = true, IsStudent = false, IsMarried = false },
            new DiscountRuleResponse { TotalDiscountAmount = 0, TotalDiscountRate = 0 }
        },

        new object[]
        {
            new DiscountRuleRequest { Price = 100, IsCitizen = false, IsStudent = true, IsMarried = true },
            new DiscountRuleResponse { TotalDiscountAmount = 0, TotalDiscountRate = 0 }
        },
        new object[]
        {
            new DiscountRuleRequest { Price = 100, IsCitizen = false, IsStudent = true, IsMarried = false },
            new DiscountRuleResponse { TotalDiscountAmount = 0, TotalDiscountRate = 0 }
        },
        new object[]
        {
            new DiscountRuleRequest { Price = 100, IsCitizen = false, IsStudent = false, IsMarried = true },
            new DiscountRuleResponse { TotalDiscountAmount = 0, TotalDiscountRate = 0 }
        },
        new object[]
        {
            new DiscountRuleRequest { Price = 100, IsCitizen = false, IsStudent = false, IsMarried = false },
            new DiscountRuleResponse { TotalDiscountAmount = 0, TotalDiscountRate = 0 }
        },
    };

    [TestCaseSource(nameof(TestData))]
    public void Execute_InputIsFromTestData_ResponseIsEqualToResult(DiscountRuleRequest request, DiscountRuleResponse response)
    {
        var result = RuleExecutor.Execute<IDiscountRule, DiscountRuleRequest, DiscountRuleResponse>(request);

        Assert.AreEqual(response.TotalDiscountAmount, result.TotalDiscountAmount);
        Assert.AreEqual(response.TotalDiscountRate, result.TotalDiscountRate);
    }
}