using System.Reflection;
using RuleDesignPattern.Core.Extensions;

namespace RuleDesignPattern.RuleEngine.Utils;

internal static class RuleHelper
{
    public static IEnumerable<Type> GetConcreteRules(Type type, RuleType ruleType)
    {
        return type.GetConcretes(Assembly.GetCallingAssembly())
            .Select(x => new
            {
                Type = x,
                Option = Attribute.GetCustomAttribute(x, typeof(RuleAttribute)) as RuleAttribute
            })
            .Where(x => x.Option?.RuleType == ruleType)
            .OrderBy(x => x.Option?.RunOrder)
            .Select(x => x.Type);
    }
}