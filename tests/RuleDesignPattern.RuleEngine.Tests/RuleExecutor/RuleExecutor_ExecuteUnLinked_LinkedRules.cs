using RuleDesignPattern.RuleEngine.Attributes;
using RuleDesignPattern.RuleEngine.Models;
using RuleDesignPattern.RuleEngine.Rules;

namespace RuleDesignPattern.RuleEngine.Tests.RuleExecutor;

[TestFixture]
internal class RuleExecutor_ExecuteUnLinked_LinkedRules
{
    [Test]
    public void ExecuteUnLinked_WithLinkedRules_LinkedRulesShouldComeAfterUnLinkedRule()
    {
        var executedLinkedRules = new List<string>
        {
            nameof(LinkedRule1),
            nameof(LinkedRule2)
        };
        var request = new RuleRequest();

        var response = RuleEngine.RuleExecutor
            .ExecuteUnLinked<IUnLinkedRule<RuleRequest, RuleResponse>, RuleRequest, RuleResponse>(request);

        var index = response.ExecutedRules.IndexOf(nameof(UnLinkedRule2)) + 1;

        CollectionAssert.AreEquivalent(response.ExecutedRules.GetRange(index, executedLinkedRules.Count),
            executedLinkedRules);
    }
}

file class RuleRequest : IRuleRequest
{
}

file class RuleResponse : IRuleResponse
{
    public List<string> ExecutedRules { get; } = Enumerable.Empty<string>().ToList();
}

file class UnLinkedRule1 : IUnLinkedRule<RuleRequest, RuleResponse>
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(nameof(UnLinkedRule1));
        return response;
    }
}

[RuleOption(typeof(LinkedRule1), typeof(LinkedRule2))]
file class UnLinkedRule2 : IUnLinkedRule<RuleRequest, RuleResponse>
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(nameof(UnLinkedRule2));
        return response;
    }
}

file class UnLinkedRule3 : IUnLinkedRule<RuleRequest, RuleResponse>
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(nameof(UnLinkedRule3));
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