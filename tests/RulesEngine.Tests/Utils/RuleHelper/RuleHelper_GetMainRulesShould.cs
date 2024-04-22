using System.Reflection;

namespace RulesEngine.Tests.Utils.RuleHelper;

[TestFixture]
internal class RuleHelper_GetMainRulesShould
{
    [Test]
    public void GetMainRules_TRuleIsIRule_OrderOfRuleListsIsSame()
    {
        var expectedRuleTypes = new List<Type> { typeof(TestRule_IRule_5), typeof(TestRule_ITestRule_10), typeof(TestRule_TestBaseRule_15) };

        var result = RulesEngine.Utils.RuleHelper.GetMainRules<IRule<TestRuleRequest, TestRuleResponse>, TestRuleRequest, TestRuleResponse>([Assembly.GetExecutingAssembly()]);
        var actualRuleTypes = result.Select(x => x.GetType()).ToList();

        CollectionAssert.AreEqual(expectedRuleTypes, actualRuleTypes);
    }

    [Test]
    public void GetMainRules_TRuleIsITestRule_OrderOfRuleListsIsSame()
    {
        var expectedRuleTypes = new List<Type> { typeof(TestRule_ITestRule_10), typeof(TestRule_TestBaseRule_15) };

        var result = RulesEngine.Utils.RuleHelper.GetMainRules<ITestRule, TestRuleRequest, TestRuleResponse>([Assembly.GetExecutingAssembly()]);
        var actualRuleTypes = result.Select(x => x.GetType()).ToList();

        CollectionAssert.AreEqual(expectedRuleTypes, actualRuleTypes);
    }

    [Test]
    public void GetMainRules_TRuleIsBaseRule_OrderOfRuleListsIsSame()
    {
        var expectedRuleTypes = new List<Type> { typeof(TestRule_TestBaseRule_15) };

        var result = RulesEngine.Utils.RuleHelper.GetMainRules<TestBaseRule, TestRuleRequest, TestRuleResponse>([Assembly.GetExecutingAssembly()]);
        var actualRuleTypes = result.Select(x => x.GetType()).ToList();

        CollectionAssert.AreEqual(expectedRuleTypes, actualRuleTypes);
    }

    [Test]
    public void GetMainRules_TRuleIsChildRule_ResultCollectionIsEmpty()
    {
        var result = RulesEngine.Utils.RuleHelper.GetMainRules<TestRule_TestBaseRule_0_TestRuleTestBaseRule15, TestRuleRequest, TestRuleResponse>([Assembly.GetExecutingAssembly()]);

        CollectionAssert.IsEmpty(result);
    }

    [Test]
    public void GetMainRules_TRuleIsRuleWithoutRuleAttribute_ResultCollectionIsEmpty()
    {
        var result = RulesEngine.Utils.RuleHelper.GetMainRules<RuleWithoutRuleAttribute, TestRuleRequest, TestRuleResponse>([Assembly.GetExecutingAssembly()]);

        CollectionAssert.IsEmpty(result);
    }

    [Test]
    public void GetMainRules_TRuleIsRuleWithoutParameterlessConstructor_ResultCollectionIsEmpty()
    {
        var result = RulesEngine.Utils.RuleHelper.GetMainRules<RuleWithoutParameterlessConstructor, TestRuleRequest, TestRuleResponse>([Assembly.GetExecutingAssembly()]);

        CollectionAssert.IsEmpty(result);
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

[Rule(5)]
file class TestRule_IRule_5 : IRule<TestRuleRequest, TestRuleResponse>
{
    public bool CanApply(TestRuleRequest request, TestRuleResponse response) => true;
    public TestRuleResponse Apply(TestRuleRequest request, TestRuleResponse response) => response;
}

[Rule(10)]
file class TestRule_ITestRule_10 : ITestRule
{
    public bool CanApply(TestRuleRequest request, TestRuleResponse response) => true;
    public TestRuleResponse Apply(TestRuleRequest request, TestRuleResponse response) => response;
}

[Rule(15)]
file class TestRule_TestBaseRule_15 : TestBaseRule;

[Rule(0, ParentRule = typeof(TestRule_TestBaseRule_15))]
file class TestRule_TestBaseRule_0_TestRuleTestBaseRule15 : TestBaseRule;

file class RuleWithoutRuleAttribute : TestBaseRule;

[Rule(0)]
file class RuleWithoutParameterlessConstructor : TestBaseRule
{
    public RuleWithoutParameterlessConstructor(string arg)
    {
    }
}