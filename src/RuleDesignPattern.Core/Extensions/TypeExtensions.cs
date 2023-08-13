using System.Reflection;

namespace RuleDesignPattern.Core.Extensions;

/// <summary>
///     Provides extension methods for <see cref="Type" />.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    ///     Searches concrete types that implement the current <see cref="Type" />
    ///     in the <see cref="Assembly" /> of the method that invoked the currently executing method.
    /// </summary>
    /// <inheritdoc cref="GetConcretes(Type, Assembly[])" />
    public static Type[] GetConcretes(this Type type) =>
        type.GetConcretes(Assembly.GetCallingAssembly());

    /// <summary>
    ///     Searches concrete types that implement the current <see cref="Type" />
    ///     in each <see cref="Assembly" /> of the <paramref name="assemblies" /> array.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>
    ///     An array of <see cref="Type" /> objects
    ///     representing all concrete elements that implement the current <paramref name="type" />.
    /// </returns>
    public static Type[] GetConcretes(this Type type, params Assembly[] assemblies) =>
        assemblies.Distinct().SelectMany(x => x.GetTypes())
            .Where(x => x is { IsAbstract: false } && x.IsAssignableTo(type)).ToArray();
}