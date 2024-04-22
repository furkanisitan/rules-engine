using System.Reflection;

namespace RulesEngine.Tests.Utils.RuleHelper;

[TestFixture]
internal class RuleHelper_GetChildRulesOfShould
{
    [Test]
    public void GetChildRulesOf_TRuleIsIRule_OrderOfRuleListsIsSame()
    {
        var expectedRuleTypes = new List<Type> { typeof(TestRule_IRule_5_TestMainRule), typeof(TestRule_ITestRule_10_TestMainRule), typeof(TestRule_TestBaseRule_15_TestMainRule) };

        var mainRule = new TestMainRule();
        var result = RulesEngine.Utils.RuleHelper.GetChildRulesOf<IRule<TestRuleRequest, TestRuleResponse>, TestRuleRequest, TestRuleResponse>(mainRule, [Assembly.GetExecutingAssembly()]);
        var actualRuleTypes = result.Select(x => x.GetType()).ToList();

        CollectionAssert.AreEqual(expectedRuleTypes, actualRuleTypes);
    }

    [Test]
    public void GetChildRulesOf_TRuleIsITestRule_OrderOfRuleListsIsSame()
    {
        var expectedRuleTypes = new List<Type> { typeof(TestRule_ITestRule_10_TestMainRule), typeof(TestRule_TestBaseRule_15_TestMainRule) };

        var mainRule = new TestMainRule();
        var result = RulesEngine.Utils.RuleHelper.GetChildRulesOf<ITestRule, TestRuleRequest, TestRuleResponse>(mainRule, [Assembly.GetExecutingAssembly()]);
        var actualRuleTypes = result.Select(x => x.GetType()).ToList();

        CollectionAssert.AreEqual(expectedRuleTypes, actualRuleTypes);
    }

    [Test]
    public void GetChildRulesOf_TRuleIsBaseRule_OrderOfRuleListsIsSame()
    {
        var expectedRuleTypes = new List<Type> { typeof(TestRule_TestBaseRule_15_TestMainRule) };

        var mainRule = new TestMainRule();
        var result = RulesEngine.Utils.RuleHelper.GetChildRulesOf<TestBaseRule, TestRuleRequest, TestRuleResponse>(mainRule, [Assembly.GetExecutingAssembly()]);
        var actualRuleTypes = result.Select(x => x.GetType()).ToList();

        CollectionAssert.AreEqual(expectedRuleTypes, actualRuleTypes);
    }
}

file class TestRuleRequest : IRuleRequest;

file class TestRuleResponse : IRuleResponse
{
    public bool CanStopRulesExecution { get; set; }
}

file interface ITestRule : IRule<TestRuleRequest, TestRuleResponse>;

file abstract class TestBaseRule : ITestRule
{
    public bool CanApply(TestRuleRequest request, TestRuleResponse response) => true;
    public TestRuleResponse Apply(TestRuleRequest request, TestRuleResponse response) => response;
}

[Rule(0)]
file class TestMainRule : TestBaseRule;

[Rule(5, ParentRule = typeof(TestMainRule))]
file class TestRule_IRule_5_TestMainRule : IRule<TestRuleRequest, TestRuleResponse>
{
    public bool CanApply(TestRuleRequest request, TestRuleResponse response) => true;
    public TestRuleResponse Apply(TestRuleRequest request, TestRuleResponse response) => response;
}

[Rule(10, ParentRule = typeof(TestMainRule))]
file class TestRule_ITestRule_10_TestMainRule : ITestRule
{
    public bool CanApply(TestRuleRequest request, TestRuleResponse response) => true;
    public TestRuleResponse Apply(TestRuleRequest request, TestRuleResponse response) => response;
}

[Rule(15, ParentRule = typeof(TestMainRule))]
file class TestRule_TestBaseRule_15_TestMainRule : TestBaseRule;