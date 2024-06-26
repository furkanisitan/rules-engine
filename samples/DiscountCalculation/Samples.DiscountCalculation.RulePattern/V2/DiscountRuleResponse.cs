﻿using RulesEngine;

namespace Samples.DiscountCalculation.RulePattern.V2;

public class DiscountRuleResponse : IRuleResponse
{
    public decimal DiscountAmount { get; private set; }
    public decimal DiscountRate { get; private set; }

    #region IRuleResponse

    public bool CanStopRulesExecution { get; set; }

    #endregion

    #region Setters

    public void SetDiscountAmount(decimal value, decimal amount)
    {
        DiscountAmount = value;
        DiscountRate = value / amount;
    }

    public void SetDiscountRate(decimal value, decimal amount)
    {
        DiscountRate = value;
        DiscountAmount = value * amount;
    }

    #endregion
}