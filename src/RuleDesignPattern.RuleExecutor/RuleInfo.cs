namespace RuleDesignPattern.RuleExecutor;

public class RuleInfo
{
    public Type RuleType { get; set; } = default!;
    public RuleOptionAttribute? RuleOption { get; set; }
}