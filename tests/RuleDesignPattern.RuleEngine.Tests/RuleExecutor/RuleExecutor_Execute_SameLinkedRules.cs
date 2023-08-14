using RuleDesignPattern.RuleEngine.Attributes;
using RuleDesignPattern.RuleEngine.Enums;
using RuleDesignPattern.RuleEngine.Models;
using RuleDesignPattern.RuleEngine.Rules;

namespace RuleDesignPattern.RuleEngine.Tests.RuleExecutor;

[TestFixture]
internal class RuleExecutor_Execute_SameLinkedRules
{
    [Test]
    public void Execute_MultipleSameLinkedRule_SameLinkedRuleMustBeExecutedOnce()
    {
        var executedLinkedRules = new List<string>
        {
            nameof(Rule1),
            nameof(LinkedRule1)
        };
        var request = new RuleRequest();

        var response = RuleEngine.RuleExecutor
            .Execute<Rule1, RuleRequest, RuleResponse>(request, new RuleResponse());

        CollectionAssert.AreEqual(executedLinkedRules, response.ExecutedRules);
    }
}

file class RuleRequest : IRuleRequest
{
}

file class RuleResponse : IRuleResponse
{
    public List<string> ExecutedRules { get; } = Enumerable.Empty<string>().ToList();
}

[RuleOption(typeof(LinkedRule1), typeof(LinkedRule1))]
file class Rule1 : IRule<RuleRequest, RuleResponse>
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(nameof(Rule1));
        return response;
    }
}

file class LinkedRule1 : ILinkedRule<RuleRequest, RuleResponse>
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(nameof(LinkedRule1));
        return response;
    }
}
