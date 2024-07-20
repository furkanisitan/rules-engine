﻿namespace RulesEngine.Tests.Common.Rules;

public abstract class DummyBaseRule : IDummyRule
{
    public bool CanApply(DummyRuleRequest request, DummyRuleResponse response)
    {
        return true;
    }

    public DummyRuleResponse Apply(DummyRuleRequest request, DummyRuleResponse response)
    {
        return response;
    }
}