using RuleDesignPattern.RuleEngine.Attributes;
using RuleDesignPattern.RuleEngine.Enums;
using RuleDesignPattern.RuleEngine.Models;
using RuleDesignPattern.RuleEngine.Rules;

namespace RuleDesignPattern.RuleEngine.Tests.RuleExecutor;

[TestFixture]
internal class RuleExecutor_ExecuteUnLinked_OrderByRuleType
{
    [Test]
    public void ExecuteUnLinked_TRuleIsIUnLinkedRule_OrderOfExecutedRuleListsIsSame()
    {
        var executedRules = new List<string>
        {
            nameof(UnLinkedRule2),
            nameof(UnLinkedRule3),
            nameof(UnLinkedRule1),
        };
        var request = new RuleRequest();

        var response = RuleEngine.RuleExecutor
            .ExecuteUnLinked<IUnLinkedRule<RuleRequest, RuleResponse>, RuleRequest, RuleResponse>(request);

        CollectionAssert.AreEqual(response.ExecutedRules, executedRules);
    }
}

file class RuleRequest : IRuleRequest
{
}

file class RuleResponse : IRuleResponse
{
    public List<string> ExecutedRules { get; } = Enumerable.Empty<string>().ToList();
}

[RuleOption(RuleType.End)]
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

[RuleOption(RuleType.Start)]
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