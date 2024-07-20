using System.Reflection.Emit;

namespace RulesEngine.Tests.Common.Utilities.Extensions;

internal static class RuleAttributeExtensions
{
    public static CustomAttributeBuilder GenerateRuleAttributeBuilder(this RuleAttribute ruleAttribute)
    {
        var attributeConstructor = typeof(RuleAttribute).GetConstructor([typeof(int)]) ??
                                   throw new InvalidOperationException($"Suitable constructor for {nameof(RuleAttribute)} not found.");

        if (ruleAttribute.ParentRule is null)
            return new CustomAttributeBuilder(attributeConstructor, [ruleAttribute.ExecutionOrder]);

        var parentRuleProperty = typeof(RuleAttribute).GetProperty(nameof(RuleAttribute.ParentRule)) ??
                                 throw new InvalidOperationException($"Property {nameof(RuleAttribute.ParentRule)} not found.");

        var namedProperties = new[] { parentRuleProperty };
        var propertyValues = new object[] { ruleAttribute.ParentRule };

        return new CustomAttributeBuilder(attributeConstructor, [ruleAttribute.ExecutionOrder], namedProperties, propertyValues);
    }
}