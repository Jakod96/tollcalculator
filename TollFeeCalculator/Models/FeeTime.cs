namespace TollFeeCalculator.Models;

// Signifies that at `Time` o'clock, the toll fee becomes `Fee`.
public class FeeTime(TimeOnly time, decimal fee)
{
    public TimeOnly Time { get; set; } = time;
    public decimal Fee { get; set; } = fee;
}