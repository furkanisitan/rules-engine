# Rule Engine
 
## Overview

This project provides an infrastructure for abstracting business logic rules from main methods. It provides a simple way to define your rules outside of the core logic of main methods, thus ensuring that any changes to the rules don't affect the main method.

## How to use it

There are basically two different rule execution logics.

* An initial rule is determined and linked rules are executed in a chained fashion.
    ```
    var result = RuleExecutor.Execute<TRule, TRequest, TResponse>(rule, request, response);
    ```
* Each of the rules of the appropriate rule type is executed independently of each other. 
    ```
    var result = RuleExecutor.ExecuteAll<TRule, TRequest, TResponse>(request, response);
    ```

> If the TRule generic type is a concrete type, only the rule of that type and other rules linked to that type by the [RuleAttribute](/src/RuleDesignPattern.RuleEngine/RuleAttribute.cs)'s `NextRules` property are executed.


## Example Usage: Discount Calculator

Suppose we have the following business rules for the discount calculation.

* An additional 20% discount should be applied to citizens who are students.
* An additional 25% discount should be applied to married citizens.
* A net 50% discount should be applied to disaster victims. (It shouldn't be plugged into any limit control.)
* Limit checks should be made at the end of the transactions. (First, the rate and then the amount should be checked.)
    - The discount rate should be at most 40%.
    - The discount amount must be a maximum of 10,000.

> [Here](/src/Samples/RuleDesignPattern.Samples.DiscountCalculation/V1/) is an example that implements these business rules with both the classical method and the rule design pattern.

Let's assume that in the future, this discount calculation logic should be changed according to the following business rules.

* Citizenship control should be removed in discounts for students.
* Citizens who are married should receive an additional 2% discount for each child. (Up to 5 children maximum.)

> [Here](/src/Samples/RuleDesignPattern.Samples.DiscountCalculation/V2/) is an example where new business requirements are added.

### Implementation Stages of Changes

* Citizenship control has been removed from [StudentDiscountRule](/src/Samples/RuleDesignPattern.Samples.DiscountCalculation/V2/RuleDesign/Rules/StudentDiscountRule.cs).
* A new rule class named [ChildDiscountRule](/src/Samples/RuleDesignPattern.Samples.DiscountCalculation/V2/RuleDesign/Rules/ChildDiscountRule.cs) has been defined.
* `ChildDiscountRule` has been added to the NextRules property of the RuleAttribute in the [MarriedDiscountRule](/src/Samples/RuleDesignPattern.Samples.DiscountCalculation/V2/RuleDesign/Rules/MarriedDiscountRule.cs) class.


## Author

**Furkan Işıtan**

* [github/furkanisitan](https://github.com/furkanisitan)
