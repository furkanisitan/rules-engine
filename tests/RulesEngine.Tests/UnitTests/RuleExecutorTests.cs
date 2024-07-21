using RulesEngine.Tests.Common.Rules;
using RulesEngine.Tests.Common.Utilities;

namespace RulesEngine.Tests.UnitTests;

[TestFixture]
public class RuleExecutorTests
{
    [Test]
    public void Execute_WithValidRules_ReturnsExpectedAppliedRuleNames()
    {
        // Arrange
        var dynamicRuleClassGenerator = new DynamicRuleClassGenerator();

        var parentRuleType1 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule1", 1);
        var parentRuleType2 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule2", 2);

        var childRuleType1OfParent1 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule1", 1, parentRuleType1);
        var childRuleType2OfParent1 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule2", 2, parentRuleType1);
        var childRuleType3OfParent2 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule3", 1, parentRuleType2);

        var childRuleType4OfChild1 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule4", 1, childRuleType1OfParent1);
        var childRuleType5OfChild2 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule5", 1, childRuleType2OfParent1);

        var expectedAppliedRuleNames = new List<string> {
            parentRuleType1.Name,
            childRuleType1OfParent1.Name,
            childRuleType4OfChild1.Name,
            childRuleType2OfParent1.Name,
            childRuleType5OfChild2.Name,
            parentRuleType2.Name,
            childRuleType3OfParent2.Name
        };

        var dummyRuleRequest = new DummyRuleRequest();
        var dummyRuleResponse = new DummyRuleResponse();
        var assemblies = new[] { dynamicRuleClassGenerator.Assembly };

        // Act
        var response = RuleExecutor.Execute<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(dummyRuleRequest, dummyRuleResponse, assemblies);

        // Assert
        CollectionAssert.AreEqual(expectedAppliedRuleNames, response.AppliedRuleNames, $"The {nameof(response.AppliedRuleNames)} of {nameof(response)} do not match the {nameof(expectedAppliedRuleNames)}.");
    }

    [Test]
    public void Execute_ParentCanStopRulesExecutionIsTrue_StopRuleExecution()
    {
        // Arrange
        var dynamicRuleClassGenerator = new DynamicRuleClassGenerator();

        var parentRuleType1 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule1", 1);
        var parentRuleType2 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule2", 2, null, true);

        var childRuleType1OfParent1 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule1", 1, parentRuleType1);
        var childRuleType2OfParent1 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule2", 2, parentRuleType1);
        dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule3", 1, parentRuleType2);

        var childRuleType4OfChild1 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule4", 1, childRuleType1OfParent1);
        var childRuleType5OfChild2 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule5", 1, childRuleType2OfParent1);

        var expectedAppliedRuleNames = new List<string> {
            parentRuleType1.Name,
            childRuleType1OfParent1.Name,
            childRuleType4OfChild1.Name,
            childRuleType2OfParent1.Name,
            childRuleType5OfChild2.Name,
            parentRuleType2.Name
        };

        var dummyRuleRequest = new DummyRuleRequest();
        var dummyRuleResponse = new DummyRuleResponse();
        var assemblies = new[] { dynamicRuleClassGenerator.Assembly };

        // Act
        var response = RuleExecutor.Execute<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(dummyRuleRequest, dummyRuleResponse, assemblies);

        // Assert
        CollectionAssert.AreEqual(expectedAppliedRuleNames, response.AppliedRuleNames, $"The {nameof(response.AppliedRuleNames)} of {nameof(response)} do not match the {nameof(expectedAppliedRuleNames)}.");
    }

    [Test]
    public void Execute_ChildCanStopRulesExecutionIsTrue_StopRuleExecution()
    {
        // Arrange
        var dynamicRuleClassGenerator = new DynamicRuleClassGenerator();

        var parentRuleType1 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule1", 1);
        var parentRuleType2 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule2", 2);

        var childRuleType1OfParent1 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule1", 1, parentRuleType1);
        var childRuleType2OfParent1 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule2", 2, parentRuleType1, true);
        dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule3", 1, parentRuleType2);

        var childRuleType4OfChild1 = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule4", 1, childRuleType1OfParent1);
        dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule5", 1, childRuleType2OfParent1);

        var expectedAppliedRuleNames = new List<string> {
            parentRuleType1.Name,
            childRuleType1OfParent1.Name,
            childRuleType4OfChild1.Name,
            childRuleType2OfParent1.Name
        };

        var dummyRuleRequest = new DummyRuleRequest();
        var dummyRuleResponse = new DummyRuleResponse();
        var assemblies = new[] { dynamicRuleClassGenerator.Assembly };

        // Act
        var response = RuleExecutor.Execute<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(dummyRuleRequest, dummyRuleResponse, assemblies);

        // Assert
        CollectionAssert.AreEqual(expectedAppliedRuleNames, response.AppliedRuleNames, $"The {nameof(response.AppliedRuleNames)} of {nameof(response)} do not match the {nameof(expectedAppliedRuleNames)}.");
    }
}