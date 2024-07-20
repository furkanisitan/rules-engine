using System.Reflection;
using System.Reflection.Emit;
using RulesEngine.Tests.Common.Rules;
using RulesEngine.Tests.Common.Utilities.Extensions;
using RulesEngine.Tests.Common.Utilities.Options;

namespace RulesEngine.Tests.Common.Utilities;

public class DynamicRuleClassGenerator
{
    private const string DynamicModuleNamePrefix = "DynamicModule";

    private readonly string _assemblyName;
    private readonly ModuleBuilder _moduleBuilder;

    public readonly Assembly Assembly;

    public DynamicRuleClassGenerator()
    {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        _assemblyName = $"{nameof(DynamicRuleClassGenerator)}_{Guid.NewGuid()}";
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(_assemblyName), AssemblyBuilderAccess.Run);
        _moduleBuilder = assemblyBuilder.DefineDynamicModule(DynamicModuleNamePrefix);
        Assembly = _moduleBuilder.Assembly;
    }

    public Type GenerateRuleClassWithoutAttribute<TBaseRule>(string className)
        where TBaseRule : IRule<DummyRuleRequest, DummyRuleResponse>
    {
        return GenerateRuleClass<TBaseRule>(new DynamicRuleClassGenerationOptions(className));
    }

    public Type GenerateRuleClass<TBaseRule>(string className, int executionOrder, Type? parentRule = null)
        where TBaseRule : IRule<DummyRuleRequest, DummyRuleResponse>
    {
        var ruleAttribute = new RuleAttribute(executionOrder) { ParentRule = parentRule };
        return GenerateRuleClass<TBaseRule>(new DynamicRuleClassGenerationOptions(className) { RuleAttribute = ruleAttribute });
    }

    public Type GenerateRuleClass<TBaseRule>(DynamicRuleClassGenerationOptions options)
        where TBaseRule : IRule<DummyRuleRequest, DummyRuleResponse>
    {
        var typeBuilder = _moduleBuilder.DefineType($"{_assemblyName}.{options.ClassName}", TypeAttributes.Public, typeof(TBaseRule));

        if (options.RuleAttribute is not null)
            typeBuilder.SetCustomAttribute(options.RuleAttribute.GenerateRuleAttributeBuilder());

        return typeBuilder.CreateType();
    }

    private Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
    {
        return args.Name.Contains(_assemblyName) ? Assembly : null;
    }
}