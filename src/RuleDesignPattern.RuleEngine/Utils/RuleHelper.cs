using System.Reflection;
using RuleDesignPattern.Core.Extensions;

namespace RuleDesignPattern.RuleEngine.Utils;

internal static class RuleHelper
{
    public static IEnumerable<Type> GetConcreteRules(Assembly[] assemblies, Type type, params RuleType[] ruleTypes)
    {
        return type.GetConcretes(assemblies)
            .Select(x => new
            {
                Type = x,
                Option = Attribute.GetCustomAttribute(x, typeof(RuleAttribute)) as RuleAttribute
            })
            .Where(x => x.Option is not null && ruleTypes.Contains(x.Option.RuleType))
            .OrderBy(x => x.Option!.RuleType).ThenBy(x => x.Option!.RunOrder)
            .Select(x => x.Type);
    }
}