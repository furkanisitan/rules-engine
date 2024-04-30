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

## Example: Discount Calculation

Suppose we have a discount calculation method that implements the following business rules.

* Student citizens should receive an additional 20% discount.
* Married citizens should receive an additional 25% discount.
* Disaster victims should receive a net 50% discount. (No limit controls should be applied). 
* Limit checks should be made at the end of the transactions. (First the rate and then the amount should be checked.)
    - The maximum discount rate can be 40%.
    - The maximum discount amount can be 10,000.

> Here are examples that apply these business rules both with the [traditional method](/samples/DiscountCalculation/Samples.DiscountCalculation.Traditional/V1/) and with the [rule pattern method](/samples/DiscountCalculation/Samples.DiscountCalculation.RulePattern/V1/).

Let's imagine that over time it is necessary to make changes to existing business rules, to add new business rules.

* Citizenship checks should be removed from student discounts.
* Married citizens -up to a maximum of 5 children- should receive an additional 2% discount for each child.

> Here are the second versions of the [traditional method](/samples/DiscountCalculation/Samples.DiscountCalculation.Traditional/V2/) and the [rule pattern method](/samples/DiscountCalculation/Samples.DiscountCalculation.RulePattern/V2/).

### Implementation Stages of the Changes

* Citizenship check has been removed from [StudentDiscountRule](/samples/DiscountCalculation/Samples.DiscountCalculation.RulePattern/V2/Rules/StudentDiscountRule.cs).
* A new rule class called [ChildDiscountRule](/samples/DiscountCalculation/Samples.DiscountCalculation.RulePattern/V2/Rules/ChildDiscountRule.cs) is defined.

## Author

**Furkan Işıtan**

* [github/furkanisitan](https://github.com/furkanisitan)
