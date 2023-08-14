using System.Reflection;
using RuleDesignPattern.Core.Extensions;
using RuleDesignPattern.RuleEngine.Attributes;

namespace RuleDesignPattern.RuleEngine;

internal static class RuleHelper
{
    public static RuleInfo[] GetConcreteRuleInfos(Type ruleType) =>
        GetConcreteRuleInfos(ruleType, Assembly.GetCallingAssembly());

    public static RuleInfo[] GetConcreteRuleInfos(Type ruleType, params Assembly[] assemblies) =>
        ruleType.GetConcretes(assemblies).Select(x => new RuleInfo
        {
            RuleType = x,
            RuleOption = Attribute.GetCustomAttribute(x, typeof(RuleOptionAttribute)) as RuleOptionAttribute
        }).ToArray();

    public static RuleInfo[] GetRuleInfos(params Type[] ruleTypes) =>
        ruleTypes.Select(x => new RuleInfo
        {
            RuleType = x,
            RuleOption = Attribute.GetCustomAttribute(x, typeof(RuleOptionAttribute)) as RuleOptionAttribute
        }).ToArray();
}