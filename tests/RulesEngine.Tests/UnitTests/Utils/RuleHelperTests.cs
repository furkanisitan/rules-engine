using RulesEngine.Tests.Common.Rules;
using RulesEngine.Tests.Common.Utilities;
using System.Diagnostics;
using System.Reflection;

namespace RulesEngine.Tests.UnitTests.Utils;

[TestFixture]
public class RuleHelperTests
{
    #region GetMainRules

    [Test]
    public void GetMainRules_WithEmptyAssemblies_ReturnsEmptyCollection()
    {
        // Arrange
        var assemblies = Array.Empty<Assembly>();

        // Act
        var mainRules = RulesEngine.Utils.RuleHelper
            .GetMainRules<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(assemblies)
            .ToList();

        // Assert
        CollectionAssert.IsEmpty(mainRules, $"{nameof(mainRules)} should be empty.");
    }

    [Test]
    public void GetMainRules_WithValidRules_ReturnsCorrectRules()
    {
        // Arrange
        var dynamicRuleClassGenerator = new DynamicRuleClassGenerator();

        var parentRuleTypes = new List<Type> {
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule1", int.MinValue),
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule2", int.MinValue),
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule3", int.MinValue)
        };

        var assemblies = new[] { dynamicRuleClassGenerator.Assembly };

        // Act
        var mainRules = RulesEngine.Utils.RuleHelper
            .GetMainRules<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(assemblies)
            .ToList();

        // Assert
        Assert.That(mainRules, Has.Count.EqualTo(parentRuleTypes.Count), $"Expected {parentRuleTypes.Count} main rules to be returned, but found {mainRules.Count}.");
        foreach (var parentRuleType in parentRuleTypes)
            Assert.That(mainRules, Has.Some.InstanceOf(parentRuleType), $"Returned rules should include at least one instance of {parentRuleType.Name}, but none was found.");
    }

    [Test]
    public void GetMainRules_WithChildRules_ReturnsOnlyParentRules()
    {
        // Arrange
        var dynamicRuleClassGenerator = new DynamicRuleClassGenerator();

        var parentRuleType = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule", int.MinValue);
        dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule", int.MinValue, parentRuleType);

        var assemblies = new[] { dynamicRuleClassGenerator.Assembly };

        // Act
        var mainRules = RulesEngine.Utils.RuleHelper
            .GetMainRules<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(assemblies)
            .ToList();

        // Assert
        Assert.That(mainRules, Has.Count.EqualTo(1), $"Expected 1 main rules to be returned, but found {mainRules.Count}.");
        Assert.That(mainRules, Has.Some.InstanceOf(parentRuleType), $"Returned rules should include at least one instance of {parentRuleType.Name}, but none was found.");
    }

    [Test]
    public void GetMainRules_WithExecutionOrder_ReturnsRulesSortedByExecutionOrder()
    {
        // Arrange
        var dynamicRuleClassGenerator = new DynamicRuleClassGenerator();

        var parentRuleTypes = new List<Type> {
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule3", 3),
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule1", 1),
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule2", 2),
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule4", 4)
        };
        var expectedSortedMainRuleTypes = new List<Type> { parentRuleTypes[1], parentRuleTypes[2], parentRuleTypes[0], parentRuleTypes[3] };

        var assemblies = new[] { dynamicRuleClassGenerator.Assembly };

        // Act
        var mainRules = RulesEngine.Utils.RuleHelper
            .GetMainRules<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(assemblies)
            .ToList();

        // Assert
        for (var i = 0; i < expectedSortedMainRuleTypes.Count; i++)
            Assert.That(mainRules[i], Is.InstanceOf(expectedSortedMainRuleTypes[i]), $"Element at index {i} should be of type {expectedSortedMainRuleTypes[i].Name}.");
    }

    [Test]
    public void GetMainRules_WithIncompatibleMainRules_ExcludesIncompatibleMainRules()
    {
        // Arrange
        var dynamicRuleClassGenerator = new DynamicRuleClassGenerator();

        dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule1", int.MinValue);
        dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule2", int.MinValue);
        dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule3", int.MinValue);
        dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule4", int.MinValue);
        dynamicRuleClassGenerator.GenerateRuleClass<DummyOtherBaseRule>("DummyOtherBaseRule1", int.MinValue);
        dynamicRuleClassGenerator.GenerateRuleClass<DummyOtherBaseRule>("DummyOtherBaseRule2", int.MinValue);

        var assemblies = new[] { dynamicRuleClassGenerator.Assembly };

        // Act
        var mainRules = RulesEngine.Utils.RuleHelper
            .GetMainRules<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(assemblies)
            .ToList();

        // Assert
        Assert.That(mainRules, Has.None.InstanceOf<DummyOtherBaseRule>(), $"Returned rules should not include any instances of {nameof(DummyOtherBaseRule)}.");
    }

    [Test]
    public void GetMainRules_PerformanceWithLargeNumberOfRules_CompletesWithinAcceptableTime()
    {
        // Arrange
        var dynamicRuleClassGenerator = new DynamicRuleClassGenerator();
        var stopwatch = new Stopwatch();

        var assemblies = new List<Assembly>();
        for (var i = 0; i < 100; i++)
        {
            var parentRuleType = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>($"DummyParentRule{i}", int.MinValue);
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>($"DummyChildRule{i}", int.MinValue, parentRuleType);
            assemblies.Add(dynamicRuleClassGenerator.Assembly);
            assemblies.Add(Assembly.GetExecutingAssembly());
        }

        // Act
        stopwatch.Start();
        RulesEngine.Utils.RuleHelper.GetMainRules<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>([.. assemblies]);
        stopwatch.Stop();

        // Assert
        const int maxAcceptableMilliseconds = 5;
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(maxAcceptableMilliseconds), $"GetMainRules method should complete in less than {maxAcceptableMilliseconds} ms for performance reasons.");
    }

    [Test]
    public void GetMainRules_WithoutRuleAttribute_ExcludesClassesWithoutRuleAttribute()
    {
        // Arrange
        var dynamicRuleClassGenerator = new DynamicRuleClassGenerator();

        dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRuleWithRuleAttribute", int.MinValue);
        var parentRuleTypeWithoutRuleAttribute = dynamicRuleClassGenerator.GenerateRuleClassWithoutAttribute<DummyBaseRule>("DummyParentRuleWithoutRuleAttribute");

        var assemblies = new[] { dynamicRuleClassGenerator.Assembly };

        // Act
        var mainRules = RulesEngine.Utils.RuleHelper
            .GetMainRules<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(assemblies)
            .ToList();

        // Assert
        Assert.That(mainRules, Has.None.InstanceOf(parentRuleTypeWithoutRuleAttribute), $"Returned rules should not include any instances of {parentRuleTypeWithoutRuleAttribute.Name}.");
    }

    #endregion GetMainRules

    #region GetChildRulesOf

    [Test]
    public void GetChildRulesOf_WithMatchingChildRules_ReturnsCorrectChildRulesForParentRule()
    {
        // Arrange
        var dynamicRuleClassGenerator = new DynamicRuleClassGenerator();

        var parentRuleType = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule", int.MinValue);
        var childRuleTypes = new List<Type> {
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule1", int.MinValue, parentRuleType),
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule2", int.MinValue, parentRuleType),
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule3", int.MinValue, parentRuleType)
        };
        var parentRule = (DummyBaseRule)Activator.CreateInstance(parentRuleType)!;

        var assemblies = new[] { dynamicRuleClassGenerator.Assembly };

        // Act
        var childRules = RulesEngine.Utils.RuleHelper
            .GetChildRulesOf<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(parentRule, assemblies)
            .ToList();

        // Assert
        Assert.That(childRules, Has.Count.EqualTo(childRuleTypes.Count), $"Expected {childRuleTypes.Count} child rules to be returned, but found {childRules.Count}.");
        foreach (var childRuleType in childRuleTypes)
            Assert.That(childRules, Has.Some.InstanceOf(childRuleType), $"Returned rules should include at least one instance of {childRuleType.Name}, but none was found.");
    }

    [Test]
    public void GetChildRulesOf_WithNonMatchingChildRules_ExcludesUnrelatedChildRules()
    {
        // Arrange
        var dynamicRuleClassGenerator = new DynamicRuleClassGenerator();

        var parentRuleType = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule", int.MinValue);
        dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyRelatedChildRule", int.MinValue, parentRuleType);
        var parentRule = (DummyBaseRule)Activator.CreateInstance(parentRuleType)!;

        var otherParentRuleType = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyOtherParentRule", int.MinValue);
        var unrelatedChildRuleType = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyUnrelatedChildRule", int.MaxValue, otherParentRuleType);

        var assemblies = new[] { dynamicRuleClassGenerator.Assembly };

        // Act
        var childRules = RulesEngine.Utils.RuleHelper
            .GetChildRulesOf<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(parentRule, assemblies)
            .ToList();

        // Assert
        Assert.That(childRules, Has.None.InstanceOf(unrelatedChildRuleType), $"Returned rules should not include any instances of {unrelatedChildRuleType.Name}.");
    }

    [Test]
    public void GetChildRulesOf_WithExecutionOrder_SortsChildRulesCorrectly()
    {
        // Arrange
        var dynamicRuleClassGenerator = new DynamicRuleClassGenerator();

        var parentRuleType = dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyParentRule", int.MinValue);
        var childRuleTypes = new List<Type> {
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule3", 3, parentRuleType),
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule1", 1, parentRuleType),
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule2", 2, parentRuleType),
            dynamicRuleClassGenerator.GenerateRuleClass<DummyBaseRule>("DummyChildRule0", 0, parentRuleType)
        };
        var expectedSortedChildRuleTypes = new List<Type> { childRuleTypes[3], childRuleTypes[1], childRuleTypes[2], childRuleTypes[0] };

        var parentRule = (DummyBaseRule)Activator.CreateInstance(parentRuleType)!;

        var assemblies = new[] { dynamicRuleClassGenerator.Assembly };

        // Act
        var childRules = RulesEngine.Utils.RuleHelper
            .GetChildRulesOf<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(parentRule, assemblies)
            .ToList();

        // Assert
        for (var i = 0; i < expectedSortedChildRuleTypes.Count; i++)
            Assert.That(childRules[i], Is.InstanceOf(expectedSortedChildRuleTypes[i]), $"Element at index {i} should be of type {expectedSortedChildRuleTypes[i].Name}.");
    }

    [Test]
    public void GetChildRulesOf_WithNullParentRule_ThrowsArgumentNullException()
    {
        // Arrange
        DummyBaseRule? parentRule = null;
        var assemblies = new[] { Assembly.GetExecutingAssembly() };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => RulesEngine.Utils.RuleHelper.GetChildRulesOf<DummyBaseRule, DummyRuleRequest, DummyRuleResponse>(parentRule, assemblies),
            "Method should throw ArgumentNullException when the parent rule is null."
        );
    }

    #endregion
}