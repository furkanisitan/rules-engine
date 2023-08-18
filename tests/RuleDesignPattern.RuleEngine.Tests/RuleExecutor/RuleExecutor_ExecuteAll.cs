namespace RuleDesignPattern.RuleEngine.Tests.RuleExecutor;

[TestFixture]
internal class RuleExecutor_ExecuteAll
{
    [Test]
    public void ExecuteIndependents_TRuleIsITestRule_OrderOfExecutedRuleListsIsSame()
    {
        var executedRules = new List<string>
        {
            typeof(RuleIndependent5).Name,
            typeof(RuleIndependent3).Name,
            typeof(RuleIndependent1).Name,
            typeof(RuleIndependent4).Name,
            typeof(RuleIndependent2).Name,
            typeof(RuleFinish4).Name,
            typeof(RuleFinish2).Name,
            typeof(RuleFinish1).Name,
            typeof(RuleFinish3).Name
        };
        var request = new RuleRequest();
        var response = new RuleResponse();

        var result = RuleEngine.RuleExecutor.ExecuteAll<ITestRule, RuleRequest, RuleResponse>(request, response);

        CollectionAssert.AreEqual(executedRules, result.ExecutedRules);
    }

    [Test]
    public void ExecuteIndependents_TRuleIsDifferentInterface_ExecutedRuleListIsEmpty()
    {
        var request = new RuleRequest();
        var response = new RuleResponse();

        var result =
            RuleEngine.RuleExecutor.ExecuteAll<IDifferentTestRule, RuleRequest, RuleResponse>(request, response);

        CollectionAssert.IsEmpty(result.ExecutedRules);
    }
}

[Rule(RuleType = RuleType.Independent)]
file class RuleIndependent1 : BaseRule
{
}

[Rule(RuleType = RuleType.Independent, RunOrder = int.MaxValue)]
file class RuleIndependent2 : BaseRule
{
}

[Rule(RuleType = RuleType.Independent, RunOrder = -55)]
file class RuleIndependent3 : BaseRule
{
}

[Rule(RuleType = RuleType.Independent, RunOrder = 55)]
file class RuleIndependent4 : BaseRule
{
}

[Rule(RuleType = RuleType.Independent, RunOrder = int.MinValue)]
file class RuleIndependent5 : BaseRule
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

file class RuleNone : BaseRule
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