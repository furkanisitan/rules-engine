using RuleDesignPattern.RuleEngine.Attributes;
using RuleDesignPattern.RuleEngine.Enums;
using RuleDesignPattern.RuleEngine.Models;
using RuleDesignPattern.RuleEngine.Rules;

namespace RuleDesignPattern.RuleEngine.Tests.RuleExecutor;

[TestFixture]
internal class RuleExecutor_Execute_InitialRule
{
    [Test]
    public void Execute_WithInitRule_OrderOfExecutedRuleListsIsSame()
    {
        var executedLinkedRules = new List<string>
        {
            nameof(Rule1),
            nameof(LinkedRule4),
            nameof(LinkedRule2),
            nameof(LinkedRule3),
            nameof(LinkedRule5),
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

[RuleOption(typeof(LinkedRule4), typeof(LinkedRule3))]
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

file class LinkedRule2 : ILinkedRule<RuleRequest, RuleResponse>
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(nameof(LinkedRule2));
        return response;
    }
}

[RuleOption(RuleType.End, typeof(LinkedRule5))]
file class LinkedRule3 : ILinkedRule<RuleRequest, RuleResponse>
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(nameof(LinkedRule3));
        return response;
    }
}

[RuleOption(RuleType.Start, typeof(LinkedRule2))]
file class LinkedRule4 : ILinkedRule<RuleRequest, RuleResponse>
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(nameof(LinkedRule4));
        return response;
    }
}

[RuleOption(typeof(LinkedRule1))]
file class LinkedRule5 : ILinkedRule<RuleRequest, RuleResponse>
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(nameof(LinkedRule5));
        return response;
    }
}