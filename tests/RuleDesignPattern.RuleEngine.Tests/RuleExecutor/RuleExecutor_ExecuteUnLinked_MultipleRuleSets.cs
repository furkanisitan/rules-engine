using RuleDesignPattern.RuleEngine.Models;
using RuleDesignPattern.RuleEngine.Rules;

namespace RuleDesignPattern.RuleEngine.Tests.RuleExecutor;

[TestFixture]
internal class RuleExecutor_ExecuteUnLinked_MultipleRuleSets
{
    [Test]
    public void ExecuteUnLinked_TRuleIsIRuleSet1_ExecutedRuleListsAreEqual()
    {
        var executedRules = new List<string>
        {
            nameof(UnLinkedRule1OfRuleSet1),
            nameof(UnLinkedRule2OfRuleSet1)
        };
        var request = new RuleRequest();

        var response = RuleEngine.RuleExecutor
            .ExecuteUnLinked<IRuleSet1, RuleRequest, RuleResponse>(request);

        Assert.That(response.ExecutedRules, Is.EquivalentTo(executedRules));
    }

    [Test]
    public void ExecuteUnLinked_TRuleIsIUnLinkedRule_ExecutedRuleListsAreEqual()
    {
        var executedRules = new List<string>
        {
            nameof(UnLinkedRule1),
            nameof(UnLinkedRule1OfRuleSet1),
            nameof(UnLinkedRule2OfRuleSet1),
            nameof(UnLinkedRule1OfRuleSet2)
        };
        var request = new RuleRequest();

        var response = RuleEngine.RuleExecutor
            .ExecuteUnLinked<IUnLinkedRule<RuleRequest, RuleResponse>, RuleRequest, RuleResponse>(request);

        CollectionAssert.AreEquivalent(response.ExecutedRules, executedRules);
    }
}

file class RuleRequest : IRuleRequest
{
}

file class RuleResponse : IRuleResponse
{
    public List<string> ExecutedRules { get; } = Enumerable.Empty<string>().ToList();
}

file interface IRuleSet1 : IUnLinkedRule<RuleRequest, RuleResponse>
{
}

file interface IRuleSet2 : IUnLinkedRule<RuleRequest, RuleResponse>
{
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

file class UnLinkedRule1OfRuleSet1 : IRuleSet1
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(nameof(UnLinkedRule1OfRuleSet1));
        return response;
    }
}

file class UnLinkedRule2OfRuleSet1 : IRuleSet1
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(nameof(UnLinkedRule2OfRuleSet1));
        return response;
    }
}

file class UnLinkedRule1OfRuleSet2 : IRuleSet2
{
    public bool CanApply(RuleRequest request, RuleResponse response)
    {
        return true;
    }

    public RuleResponse Apply(RuleRequest request, RuleResponse response)
    {
        response.ExecutedRules.Add(nameof(UnLinkedRule1OfRuleSet2));
        return response;
    }
}