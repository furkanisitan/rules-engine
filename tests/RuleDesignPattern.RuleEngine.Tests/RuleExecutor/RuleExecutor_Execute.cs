namespace RuleDesignPattern.RuleEngine.Tests.RuleExecutor;

[TestFixture]
internal class RuleExecutor_Execute
{
    [Test]
    public void Execute_TRuleIsAbstractType_OrderOfExecutedRuleListsIsSame()
    {
        var executedRules = new List<string>
        {
            typeof(RuleNone1).Name,
            typeof(RuleNone2).Name,
            typeof(RuleNone4).Name,
            typeof(RuleNone3).Name,
            typeof(RuleNone5).Name,
            typeof(RuleFinish4).Name,
            typeof(RuleFinish2).Name,
            typeof(RuleFinish1).Name,
            typeof(RuleFinish3).Name
        };
        var request = new RuleRequest();
        var response = new RuleResponse();
        var rule = new RuleNone1();

        var result = RuleEngine.RuleExecutor.Execute<ITestRule, RuleRequest, RuleResponse>(rule, request, response);

        CollectionAssert.AreEqual(executedRules, result.ExecutedRules);
    }

    [Test]
    public void Execute_TRuleIsConcreteType_OrderOfExecutedRuleListsIsSame()
    {
        var executedRules = new List<string>
        {
            typeof(RuleNone1).Name,
            typeof(RuleNone2).Name,
            typeof(RuleNone4).Name,
            typeof(RuleNone3).Name,
            typeof(RuleNone5).Name
        };
        var request = new RuleRequest();
        var response = new RuleResponse();
        var rule = new RuleNone1();

        var result = RuleEngine.RuleExecutor.Execute(rule, request, response);

        CollectionAssert.AreEqual(executedRules, result.ExecutedRules);
    }
}

[Rule(typeof(RuleNone2), typeof(RuleNone3))]
file class RuleNone1 : BaseRule
{
}

[Rule(typeof(RuleNone4))]
file class RuleNone2 : BaseRule
{
}

[Rule(typeof(RuleNone5))]
file class RuleNone3 : BaseRule
{
}

file class RuleNone4 : BaseRule
{
}

file class RuleNone5 : BaseRule
{
}

[Rule(RuleType = RuleType.Finish, RunOrder = 3)]
file class RuleFinish1 : BaseRule
{
}

[Rule(RuleType = RuleType.Finish, RunOrder = 2)]
file class RuleFinish2 : BaseRule
{
}

[Rule(RuleType = RuleType.Finish, RunOrder = 4)]
file class RuleFinish3 : BaseRule
{
}

[Rule(RuleType = RuleType.Finish, RunOrder = 1)]
file class RuleFinish4 : BaseRule
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