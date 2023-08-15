using RuleDesignPattern.RuleEngine.Attributes;

namespace RuleDesignPattern.RuleEngine.Tests.RuleExecutor;

[TestFixture]
internal class RuleExecutor_Execute
{
    [Test]
    public void Execute_WithoutResponseObject_OrderOfExecutedRuleListsIsSame()
    {
        var executedRules = new List<string>
        {
            typeof(Rule1).Name,
            typeof(Rule2).Name,
            typeof(Rule4).Name,
            typeof(Rule3).Name,
            typeof(Rule5).Name
        };
        var request = new RuleRequest();
        var rule = new Rule1();

        var result = RuleEngine.RuleExecutor.Execute<ITestRule, RuleRequest, RuleResponse>(rule, request);

        CollectionAssert.AreEqual(executedRules, result.ExecutedRules);
    }

    [Test]
    public void Execute_WithResponseObject_OrderOfExecutedRuleListsIsSame()
    {
        var executedRules = new List<string>
        {
            typeof(Rule1).Name,
            typeof(Rule2).Name,
            typeof(Rule4).Name,
            typeof(Rule3).Name,
            typeof(Rule5).Name
        };
        var request = new RuleRequest();
        var response = new RuleResponse();
        var rule = new Rule1();

        var result = RuleEngine.RuleExecutor.Execute(rule, request, response);

        CollectionAssert.AreEqual(executedRules, result.ExecutedRules);
    }

    [Test]
    public void Execute_WithResponseObject_OrderOfExecutedRuleListsIsNotSame()
    {
        var executedRules = new List<string>
        {
            typeof(Rule1).Name,
            typeof(Rule2).Name,
            typeof(Rule3).Name,
            typeof(Rule4).Name,
            typeof(Rule5).Name
        };
        var request = new RuleRequest();
        var response = new RuleResponse();
        var rule = new Rule1();

        var result = RuleEngine.RuleExecutor.Execute(rule, request, response);

        CollectionAssert.AreNotEqual(executedRules, result.ExecutedRules);
    }
}

[NextRules(typeof(Rule2), typeof(Rule3))]
file class Rule1 : BaseRule
{
}

[NextRules(typeof(Rule4))]
file class Rule2 : BaseRule
{
}

[NextRules(typeof(Rule5))]
file class Rule3 : BaseRule
{
}

file class Rule4 : BaseRule
{
}

file class Rule5 : BaseRule
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
}

file interface ITestRule : IRule<RuleRequest, RuleResponse>
{
}