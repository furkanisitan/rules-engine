namespace Samples.DiscountCalculation.Traditional.V1;

public record DiscountCalculationRequest(
    decimal Amount,
    bool IsCitizen,
    bool IsStudent,
    bool IsMarried,
    bool IsVictim
);