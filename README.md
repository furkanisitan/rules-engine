# Rule Engine
 
## Overview

This library provides an infrastructure that helps implement the rule design pattern, to abstract business logic rules from main methods. Rules are defined outside of the core logic of the main methods. So that any changes to the rules do not affect the main method.



## How to use it

You can find an example usage [here](/samples/DiscountCalculation/Samples.DiscountCalculation.RulePattern).

- Define request and response classes.

    ```csharp
    public record DiscountRuleRequest(decimal Amount) : IRuleRequest;

    public class DiscountRuleResponse : IRuleResponse
    {
        public bool CanStopRulesExecution { get; set; }
    }
    ```
- Define an interface that inherits the IRule interface.

    ```csharp
    public interface IDiscountRule : IRule<DiscountRuleRequest, DiscountRuleResponse>;
    ```
- Define the rules.

    ```csharp
    [Rule(2)]
    public class StudentDiscountRule : IDiscountRule
    {
        private const decimal Rate = .2M;

        public bool CanApply(DiscountRuleRequest request, DiscountRuleResponse response)
        {
            return request is { IsCitizen: true, IsStudent: true };
        }

        public DiscountRuleResponse Apply(DiscountRuleRequest request, DiscountRuleResponse response)
        {
            response.SetDiscountRate(response.DiscountRate + Rate, request.Amount);
            return response;
        }
    }
    ```
- Execute the rules.

    ```csharp
    var response = RuleExecutor.Execute<IDiscountRule, DiscountRuleRequest, DiscountRuleResponse>(request);
    ```

> Child rules are executed only when the parent rule on which they depend is executed.

> Child Rule: A rule that has a parent rule.

## Sample: Discount Calculator

Suppose we have the following business rules for the discount calculation.

* An additional 20% discount should be applied to citizens who are students.
* An additional 25% discount should be applied to married citizens.
* A net 50% discount should be applied to disaster victims. (It shouldn't be plugged into any limit control.)
* Limit checks should be made at the end of the transactions. (First, the rate and then the amount should be checked.)
    - The discount rate should be at most 40%.
    - The discount amount must be a maximum of 10,000 units.

> Here are examples that apply these business rules both with the [traditional method](/samples/DiscountCalculation/Samples.DiscountCalculation.Traditional/V1/) and with the [rule pattern method](/samples/DiscountCalculation/Samples.DiscountCalculation.RulePattern/V1/).

Let's assume that in the future, this discount calculation logic should be changed according to the following business rules.

* Citizenship control should be removed in discounts for students.
* Citizens who are married should receive an additional 2% discount for each child. (Up to 5 children maximum.)

> Here are the second versions of the [traditional method](/samples/DiscountCalculation/Samples.DiscountCalculation.Traditional/V2/) and the [rule pattern method](/samples/DiscountCalculation/Samples.DiscountCalculation.RulePattern/V2/).

### Implementation Stages of Changes

* Citizenship control has been removed from [StudentDiscountRule](/samples/DiscountCalculation/Samples.DiscountCalculation.RulePattern/V2/Rules/StudentDiscountRule.cs).
* A new rule class named [ChildDiscountRule](/samples/DiscountCalculation/Samples.DiscountCalculation.RulePattern/V2/Rules/ChildDiscountRule.cs) has been defined.

## Author

**Furkan Işıtan**

* [github/furkanisitan](https://github.com/furkanisitan)
