namespace RulesEngine.Tests.Common.Rules;

public abstract class DummyOtherBaseRule : IDummyRule
{
    public virtual bool CanApply(DummyRuleRequest request, DummyRuleResponse response)
    {
        return true;
    }

    public virtual DummyRuleResponse Apply(DummyRuleRequest request, DummyRuleResponse response)
    {
        response.AppliedRuleNames.Add(GetType().Name);
        return response;
    }
}