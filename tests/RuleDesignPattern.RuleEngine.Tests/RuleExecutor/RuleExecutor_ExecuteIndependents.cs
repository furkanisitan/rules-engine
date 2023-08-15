using System.Reflection;
using RuleDesignPattern.RuleEngine.Attributes;

namespace RuleDesignPattern.RuleEngine.Tests.RuleExecutor;

[TestFixture]
internal class RuleExecutor_ExecuteIndependents_IndependentRule
{
    [Test]
    public void ExecuteIndependents_WithIndependentRuleAttribute_ExecutedRuleListsAreEquivalent()
    {
        var executedRules = new List<string>
        {
            typeof(Rule5).Name,
            typeof(Rule4).Name,
            typeof(Rule3).Name,
            typeof(Rule2).Name,
            typeof(Rule1).Name
        };
        var request = new RuleRequest();
        var response = new RuleResponse();
        var assembly = Assembly.GetExecutingAssembly();

        var result = RuleEngine.RuleExecutor.ExecuteIndependents<ITestRule, RuleRequest, RuleResponse>(
            request, response, assembly
        );

        CollectionAssert.AreEquivalent(executedRules, result.ExecutedRules);
    }

    [Test]
    public void ExecuteIndependents_WithIndependentRuleAttribute_OrderOfExecutedRuleListsIsSame()
    {
        var executedRules = new List<string>
        {
            typeof(Rule5).Name,
            typeof(Rule3).Name,
            typeof(Rule1).Name,
            typeof(Rule4).Name,
            typeof(Rule2).Name
        };
        var request = new RuleRequest();
        var response = new RuleResponse();
        var assembly = Assembly.GetExecutingAssembly();

        var result = RuleEngine.RuleExecutor.ExecuteIndependents<ITestRule, RuleRequest, RuleResponse>(
            request, response, assembly
        );

        CollectionAssert.AreEqual(executedRules, result.ExecutedRules);
    }

    [Test]
    public void ExecuteIndependents_TRuleIsDifferentInterface_ExecutedRuleListIsEmpty()
    {
        var request = new RuleRequest();
        var response = new RuleResponse();
        var assembly = Assembly.GetExecutingAssembly();

        var result = RuleEngine.RuleExecutor.ExecuteIndependents<IDifferentTestRule, RuleRequest, RuleResponse>(
            request, response, assembly
        );

        CollectionAssert.IsEmpty(result.ExecutedRules);
    }
}

[IndependentRule]
file class Rule1 : BaseRule
{
}

[IndependentRule(RunOrder = int.MaxValue)]
file class Rule2 : BaseRule
{
}

[IndependentRule(RunOrder = -10)]
file class Rule3 : BaseRule
{
}

[IndependentRule(RunOrder = 55)]
file class Rule4 : BaseRule
{
}

[IndependentRule(RunOrder = int.MinValue)]
file class Rule5 : BaseRule
{
}

file class Rule6 : BaseRule
{
}

file abstract class BaseRule : ITestRule
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(GetType().Name);
        return response;
    }
}

file class RuleRequest : IRuleRequest
{
}

file class RuleResponse : IRuleResponse
{
    public List<string> ExecutedRules { get; } = Enumerable.Empty<string>().ToList();
    public bool StopRuleExecution { get; set; }
}

file interface ITestRule : IRule<RuleRequest, RuleResponse>
{
}

file interface IDifferentTestRule : IRule<RuleRequest, RuleResponse>
{
}