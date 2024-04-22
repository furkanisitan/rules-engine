namespace Samples.DiscountCalculation.Traditional.V2;

public record DiscountCalculationRequest(
    decimal Amount,
    bool IsCitizen,
    bool IsStudent,
    bool IsMarried,
    bool IsVictim,
    byte ChildCount
);