using RulesEngine.Tests.Common.Rules;
using RulesEngine.Tests.Common.Utilities.Extensions;
using RulesEngine.Tests.Common.Utilities.Options;
using System.Reflection;
using System.Reflection.Emit;

namespace RulesEngine.Tests.Common.Utilities;

/// <summary>
///     Provides functionality to dynamically generate rule classes at runtime.
/// </summary>
public class DynamicRuleClassGenerator
{
    private const string DynamicModuleNamePrefix = "DynamicModule";

    private readonly string _assemblyName;
    private readonly ModuleBuilder _moduleBuilder;

    /// <summary>
    ///     Gets the dynamically generated assembly containing the rule classes.
    /// </summary>
    public readonly Assembly Assembly;

    public DynamicRuleClassGenerator()
    {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        _assemblyName = $"{nameof(DynamicRuleClassGenerator)}_{Guid.NewGuid()}";
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(_assemblyName), AssemblyBuilderAccess.Run);
        _moduleBuilder = assemblyBuilder.DefineDynamicModule(DynamicModuleNamePrefix);
        Assembly = _moduleBuilder.Assembly;
    }

    /// <summary>
    ///     Generates a rule class of the specified type without attaching any custom attributes to it.
    /// </summary>
    /// <typeparam name="TBaseRule">The base type of the rule. This type must implement the IRule interface.</typeparam>
    /// <param name="className">The name of the class to be generated.</param>
    /// <returns>A <see cref="Type" /> object representing the dynamically generated rule class.</returns>
    public Type GenerateRuleClassWithoutAttribute<TBaseRule>(string className)
        where TBaseRule : IRule<DummyRuleRequest, DummyRuleResponse>
    {
        return GenerateRuleClass<TBaseRule>(new DynamicRuleClassGenerationOptions(className));
    }

    /// <summary>
    ///     Generates a rule class of the specified type, attaching a custom RuleAttribute to it.
    /// </summary>
    /// <typeparam name="TBaseRule">The base type of the rule. This type must implement the IRule interface.</typeparam>
    /// <param name="className">The name of the class to be generated.</param>
    /// <param name="executionOrder">The execution order of the rule.</param>
    /// <param name="parentRule">Optional. The type of the parent rule, if any. This is used to establish rule hierarchy.</param>
    /// <returns>
    ///     A <see cref="Type" /> object representing the dynamically generated rule class
    ///     with the custom <see cref="RuleAttribute" />.
    /// </returns>
    public Type GenerateRuleClass<TBaseRule>(string className, int executionOrder, Type? parentRule = null)
        where TBaseRule : IRule<DummyRuleRequest, DummyRuleResponse>
    {
        return GenerateRuleClass<TBaseRule>(className, executionOrder, parentRule, false);
    }

    /// <summary>
    ///     Generates a rule class of the specified type, attaching a custom RuleAttribute to it.
    /// </summary>
    /// <typeparam name="TBaseRule">The base type of the rule. This type must implement the IRule interface.</typeparam>
    /// <param name="className">The name of the class to be generated.</param>
    /// <param name="executionOrder">The execution order of the rule.</param>
    /// <param name="parentRule">Optional. The type of the parent rule, if any. This is used to establish rule hierarchy.</param>
    /// <param name="canStopRuleExecution">Indicates whether the generated rule can stop the execution of subsequent rules.</param>
    /// <returns>
    ///     A <see cref="Type" /> object representing the dynamically generated rule class
    ///     with the custom <see cref="RuleAttribute" />.
    /// </returns>
    public Type GenerateRuleClass<TBaseRule>(string className, int executionOrder, Type? parentRule, bool canStopRuleExecution)
        where TBaseRule : IRule<DummyRuleRequest, DummyRuleResponse>
    {
        var options = new DynamicRuleClassGenerationOptions(className) {
            RuleAttribute = new RuleAttribute(executionOrder) { ParentRule = parentRule },
            CanStopRulesExecution = canStopRuleExecution
        };
        return GenerateRuleClass<TBaseRule>(options);
    }

    /// <summary>
    ///     Generates a rule class based on the provided <see cref="DynamicRuleClassGenerationOptions" />.
    /// </summary>
    /// <typeparam name="TBaseRule">The base type of the rule. This type must implement the IRule interface.</typeparam>
    /// <param name="options">The options for generating the rule class.</param>
    /// <returns>A <see cref="Type" /> object representing the dynamically generated rule class.</returns>
    public Type GenerateRuleClass<TBaseRule>(DynamicRuleClassGenerationOptions options)
        where TBaseRule : IRule<DummyRuleRequest, DummyRuleResponse>
    {
        var typeBuilder = _moduleBuilder.DefineType($"{_assemblyName}.{options.ClassName}", TypeAttributes.Public, typeof(TBaseRule));

        if (options.GenerateRuleAttributeBuilder() is { } ruleAttributeBuilder)
            typeBuilder.SetCustomAttribute(ruleAttributeBuilder);

        if (options.GenerateApplyMethodBuilder(typeBuilder) is not { } methodBuilder)
            return typeBuilder.CreateType();

        var methodInfo = typeof(TBaseRule).GetMethod(nameof(IRule<IRuleRequest, IRuleResponse>.Apply)) ??
                         throw new InvalidOperationException($"{nameof(IRule<IRuleRequest, IRuleResponse>.Apply)} method not found in {typeof(TBaseRule).Name}.");
        typeBuilder.DefineMethodOverride(methodBuilder, methodInfo);

        return typeBuilder.CreateType();
    }

    private Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
    {
        return args.Name.Contains(_assemblyName) ? Assembly : null;
    }
}