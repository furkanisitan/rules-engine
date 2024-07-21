using RulesEngine.Tests.Common.Rules;
using RulesEngine.Tests.Common.Utilities.Options;
using System.Reflection;
using System.Reflection.Emit;

namespace RulesEngine.Tests.Common.Utilities.Extensions;

/// <summary>
///     Provides extension methods for <see cref="DynamicRuleClassGenerationOptions" />.
/// </summary>
internal static class DynamicRuleClassGenerationOptionsExtensions
{
    /// <summary>
    ///     Generates a <see cref="CustomAttributeBuilder" /> based on the <see cref="RuleAttribute" /> properties
    ///     defined in the <see cref="DynamicRuleClassGenerationOptions" />.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>
    ///     A <see cref="CustomAttributeBuilder" /> object if <see cref="RuleAttribute" /> is defined,
    ///     otherwise <see langword="null" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">If the required constructor or properties are not found.</exception>
    public static CustomAttributeBuilder? GenerateRuleAttributeBuilder(this DynamicRuleClassGenerationOptions options)
    {
        if (options.RuleAttribute is null)
            return null;

        var attributeConstructor = typeof(RuleAttribute).GetConstructor([typeof(int)]) ??
                                   throw new InvalidOperationException($"Suitable constructor for {nameof(RuleAttribute)} not found.");

        if (options.RuleAttribute.ParentRule is null)
            return new CustomAttributeBuilder(attributeConstructor, [options.RuleAttribute.ExecutionOrder]);

        var parentRuleProperty = typeof(RuleAttribute).GetProperty(nameof(RuleAttribute.ParentRule)) ??
                                 throw new InvalidOperationException($"Property {nameof(RuleAttribute.ParentRule)} not found.");

        var namedProperties = new[] { parentRuleProperty };
        var propertyValues = new object[] { options.RuleAttribute.ParentRule };

        return new CustomAttributeBuilder(attributeConstructor, [options.RuleAttribute.ExecutionOrder], namedProperties, propertyValues);
    }

    /// <summary>
    ///     Generates a <see cref="MethodBuilder" /> for the <see cref="IRule{TRequest,TResponse}.Apply" /> method
    ///     based on the provided <see cref="DynamicRuleClassGenerationOptions" />.
    /// </summary>
    /// <remarks>
    ///     This method generates the following code:<br />
    ///     <code>
    ///         response.CanStopRulesExecution = true;<br />
    ///         response.AppliedRuleNames.Add(GetType().Name);<br />
    ///         return response;
    ///     </code>
    /// </remarks>
    /// <param name="options"></param>
    /// <param name="typeBuilder">The type builder to which the 'Apply' method will be added.</param>
    /// <returns>
    ///     A <see cref="MethodBuilder" /> object representing the dynamically created 'Apply' method,
    ///     or <see langword="null" /> if the <see cref="IRuleResponse.CanStopRulesExecution" /> option is not enabled.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If any required properties or methods are not found
    ///     during the generation process.
    /// </exception>
    public static MethodBuilder? GenerateApplyMethodBuilder(this DynamicRuleClassGenerationOptions options, TypeBuilder typeBuilder)
    {
        if (!options.CanStopRulesExecution)
            return null;

        var canStopRulesExecutionProperty = typeof(DummyRuleResponse).GetProperty(nameof(DummyRuleResponse.CanStopRulesExecution)) ??
                                            throw new InvalidOperationException($"Property {nameof(DummyRuleResponse.CanStopRulesExecution)} not found.");

        var canStopRulesExecutionSetterMethodInfo = canStopRulesExecutionProperty.GetSetMethod() ??
                                                    throw new InvalidOperationException($"Setter method for property {nameof(DummyRuleResponse.CanStopRulesExecution)} not found.");


        var appliedRuleNamesProperty = typeof(DummyRuleResponse).GetProperty(nameof(DummyRuleResponse.AppliedRuleNames)) ??
                                       throw new InvalidOperationException($"Property {nameof(DummyRuleResponse.AppliedRuleNames)} not found.");


        var appliedRuleNamesGetterMethodInfo = appliedRuleNamesProperty.GetGetMethod() ??
                                               throw new InvalidOperationException($"Getter method for property {nameof(DummyRuleResponse.AppliedRuleNames)} not found.");


        var methodBuilder = typeBuilder.DefineMethod(
            nameof(IRule<DummyRuleRequest, DummyRuleResponse>.Apply),
            MethodAttributes.Public | MethodAttributes.Virtual,
            typeof(DummyRuleResponse),
            [typeof(DummyRuleRequest), typeof(DummyRuleResponse)]
        );

        var ilGenerator = methodBuilder.GetILGenerator();

        // Set the CanStopRuleExecution property of the response object to true
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ldc_I4_1);
        ilGenerator.Emit(OpCodes.Callvirt, canStopRulesExecutionSetterMethodInfo);

        // Add the class name to the AppliedRuleNames property
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Callvirt, appliedRuleNamesGetterMethodInfo);
        ilGenerator.Emit(OpCodes.Ldstr, options.ClassName);
        ilGenerator.Emit(OpCodes.Callvirt, typeof(List<string>).GetMethod("Add")!);

        // Return the response object
        ilGenerator.Emit(OpCodes.Ldarg_2);
        ilGenerator.Emit(OpCodes.Ret);

        return methodBuilder;
    }
}