using System.Reflection;
using RuleDesignPattern.Core.Extensions;
using RuleDesignPattern.RuleEngine.Attributes;

namespace RuleDesignPattern.RuleEngine;

internal static class RuleHelper
{
    public static IEnumerable<RuleInfo> GetConcreteRuleInfos(Type ruleType) =>
        GetConcreteRuleInfos(ruleType, Assembly.GetCallingAssembly());

    public static IEnumerable<RuleInfo> GetConcreteRuleInfos(Type ruleType, params Assembly[] assemblies) =>
        ruleType.GetConcretes(assemblies).Select(x => new RuleInfo
        {
            RuleType = x,
            RuleOption = Attribute.GetCustomAttribute(x, typeof(RuleOptionAttribute)) as RuleOptionAttribute
        });
}