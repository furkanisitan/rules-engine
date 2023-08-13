using RuleDesignPattern.RuleEngine.Attributes;

namespace RuleDesignPattern.RuleEngine;

internal class RuleInfo
{
    public Type RuleType { get; set; } = default!;
    public RuleOptionAttribute? RuleOption { get; set; }
}