using System.Reflection;

namespace RulesEngine.Tests.RuleExecutor;

[TestFixture]
internal class RuleExecutor_ExecuteShould
{
    [Test]
    public void GetMainRules_TRuleIsBaseRule_OrderOfRuleListsIsSame()
    {
        var expectedRuleTypes = new List<Type>
        {
            typeof(TestRule_TestBaseRule_5),
            typeof(TestRule_TestBaseRule_0_TestRuleTestBaseRule5),
            typeof(TestRule_TestBaseRule_5_TestRuleTestBaseRule5),
            typeof(TestRule_TestBaseRule_10),
            typeof(TestRule_TestBaseRule_0_TestRuleTestBaseRule10),
            typeof(TestRule_TestBaseRule_5_TestRuleTestBaseRule10),
            typeof(TestRule_TestBaseRule_10_TestRuleTestBaseRule10),
            typeof(TestRule_TestBaseRule_15)
        };

        var ruleResponse = RulesEngine.RuleExecutor.Execute<TestBaseRule, TestRuleRequest, TestRuleResponse>(new TestRuleRequest(), [Assembly.GetExecutingAssembly()]);

        CollectionAssert.AreEqual(expectedRuleTypes, ruleResponse.ExecutedRuleTypes);
    }
}

file class TestRuleRequest : IRuleRequest;

file class TestRuleResponse : IRuleResponse
{
    public List<Type> ExecutedRuleTypes { get; } = [];
    public bool CanStopRulesExecution { get; set; }
}

file abstract class TestBaseRule : IRule<TestRuleRequest, TestRuleResponse>
{
    public bool CanApply(TestRuleRequest request, TestRuleResponse response) => true;

    public TestRuleResponse Apply(TestRuleRequest request, TestRuleResponse response)
    {
        response.ExecutedRuleTypes.Add(GetType());
        return response;
    }
}

[Rule(5)]
file class TestRule_TestBaseRule_5 : TestBaseRule;

[Rule(10)]
file class TestRule_TestBaseRule_10 : TestBaseRule;

[Rule(15)]
file class TestRule_TestBaseRule_15 : TestBaseRule;

[Rule(0, ParentRule = typeof(TestRule_TestBaseRule_5))]
file class TestRule_TestBaseRule_0_TestRuleTestBaseRule5 : TestBaseRule;

[Rule(5, ParentRule = typeof(TestRule_TestBaseRule_5))]
file class TestRule_TestBaseRule_5_TestRuleTestBaseRule5 : TestBaseRule;

[Rule(0, ParentRule = typeof(TestRule_TestBaseRule_10))]
file class TestRule_TestBaseRule_0_TestRuleTestBaseRule10 : TestBaseRule;

[Rule(5, ParentRule = typeof(TestRule_TestBaseRule_10))]
file class TestRule_TestBaseRule_5_TestRuleTestBaseRule10 : TestBaseRule;

[Rule(10, ParentRule = typeof(TestRule_TestBaseRule_10))]
file class TestRule_TestBaseRule_10_TestRuleTestBaseRule10 : TestBaseRule;