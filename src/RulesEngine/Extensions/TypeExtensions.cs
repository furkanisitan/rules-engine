using System.Reflection;

namespace RulesEngine.Extensions;

/// <summary>
///     Provides extension methods for <see cref="Type" />.
/// </summary>
internal static class TypeExtensions
{
    /// <summary>
    ///     Searches concrete types that implement the current <paramref name="type" />
    ///     in the <see cref="Assembly" /> of the method that invoked the currently executing method.
    /// </summary>
    /// <inheritdoc cref="GetConcretes(Type, Assembly[])" />
    public static IEnumerable<Type> GetConcretes(this Type type)
    {
        return GetConcretes(type, Assembly.GetCallingAssembly());
    }

    /// <summary>
    ///     Searches concrete types that implement the current <paramref name="type" />
    ///     in each <see cref="Assembly" /> of the <paramref name="assemblies" /> array.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>
    ///     A collection of <see cref="Type" /> objects representing all concrete types
    ///     that implement the current <paramref name="type" />.
    /// </returns>
    public static IEnumerable<Type> GetConcretes(this Type type, params Assembly[] assemblies)
    {
        return assemblies
            .Distinct()
            .SelectMany(x => x.GetTypes())
            .Where(x => x is { IsAbstract: false } && x.IsAssignableTo(type));
    }

    /// <summary>
    ///     Searches concrete types that implement the current <paramref name="type" />
    ///     in the <see cref="Assembly" /> of the method that invoked the currently executing method
    ///     and have the <typeparamref name="TAttribute" /> attribute.
    /// </summary>
    /// <inheritdoc cref="GetConcretesWithAttribute{TAttribute}(Type, Assembly[])" />
    public static IEnumerable<(Type Type, TAttribute Attribute)> GetConcretesWithAttribute<TAttribute>(this Type type)
        where TAttribute : Attribute
    {
        return GetConcretesWithAttribute<TAttribute>(type, Assembly.GetCallingAssembly());
    }

    /// <summary>
    ///     Searches concrete types that implement the current <paramref name="type" />
    ///     in each <see cref="Assembly" /> of the <paramref name="assemblies" /> array
    ///     and have the <typeparamref name="TAttribute" /> attribute.
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="type"></param>
    /// <param name="assemblies"></param>
    /// <returns>
    ///     A collection of <see cref="Tuple" /> objects containing all concrete types
    ///     that implement the current <paramref name="type" />
    ///     and have the <typeparamref name="TAttribute" /> attribute.
    /// </returns>
    public static IEnumerable<(Type Type, TAttribute Attribute)> GetConcretesWithAttribute<TAttribute>(this Type type, params Assembly[] assemblies)
        where TAttribute : Attribute
    {
        return type
            .GetConcretes(assemblies)
            .Select(x => (Type: x, Attribute: x.GetCustomAttribute<TAttribute>()))
            .Where(x => x.Attribute is not null)!;
    }

    /// <summary>
    ///     Gets the parameterless constructor of the current <paramref name="type" />.
    /// </summary>
    /// <param name="type"></param>
    /// <returns>
    ///     A <see cref="ConstructorInfo" /> object representing the parameterless constructor, if found;
    ///     otherwise, <see langword="null" />.
    /// </returns>
    public static ConstructorInfo? GetParameterlessConstructor(this Type type)
    {
        return type.GetConstructor(Type.EmptyTypes);
    }

    /// <summary>
    ///     Determines whether the current <paramref name="type" /> has a parameterless constructor.
    /// </summary>
    /// <param name="type"></param>
    /// <returns>
    ///     <see langword="true" />, if the current <paramref name="type" /> has a parameterless constructor;
    ///     otherwise, <see langword="false" />.
    /// </returns>
    public static bool HasParameterlessConstructor(this Type type)
    {
        return type.GetParameterlessConstructor() is not null;
    }
}