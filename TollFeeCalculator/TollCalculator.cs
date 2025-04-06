using TollFeeCalculator.Enums;
using TollFeeCalculator.Helpers;

namespace TollFeeCalculator;

public class TollCalculator
{
    //These values should of course be stored in a database or as appsettings depending on how the application is meant to be used.
    private static readonly TimeSpan FeeGracePeriod = TimeSpan.FromHours(1);
    private const int MaximumFeePerDay = 60;

    /// <summary>
    /// Calculates the toll fee to pay for one day.
    /// </summary>
    /// <param name="vehicleType">The type of the vehicle.</param>
    /// <param name="dateTimes">Times that the vehicle has crossed a toll point within one day.</param>
    /// <returns>The fee to be paid, in SEK.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If <c>dateTimes</c> contains times from multiple days.</exception>
    public static decimal GetTollFee(VehicleType vehicleType, List<DateTime> dateTimes)
    {
        if (!AllDateTimesOnTheSameDay(dateTimes))
        {
            throw new ArgumentOutOfRangeException(nameof(dateTimes));
        }

        if (FeeCalculatorHelper.IsTollFreeVehicle(vehicleType))
        {
            return 0;
        }

        if (dateTimes.Count == 0 || TollCalendarHelper.IsTollFreeDay(DateOnly.FromDateTime(dateTimes.First())))
        {
            return 0;
        }

        var fee = PartitionTollEvents(dateTimes).Select(FeeCalculatorHelper.GetTollFeeForPeriod).Sum();
        return Math.Min(fee, MaximumFeePerDay);
    }

    // Partitions toll events into groups, resulting in one group per period, as defined by `FeeGracePeriod`.
    private static IEnumerable<List<DateTime>> PartitionTollEvents(IEnumerable<DateTime> dateTimes)
    {
        var currentPeriod = new List<DateTime>();

        foreach (var time in dateTimes.Order())
        {
            if (currentPeriod.Count == 0)
            {
                currentPeriod.Add(time);
            }
            else if (time < currentPeriod.First() + FeeGracePeriod)
            {
                currentPeriod.Add(time);
            }
            else
            {
                yield return currentPeriod;
                currentPeriod = new() { time };
            }
        }

        yield return currentPeriod;
    }

    private static bool AllDateTimesOnTheSameDay(List<DateTime> dateTimes)
    {
        var day = dateTimes.FirstOrDefault().Date;
        return dateTimes.All(d => d.Date == day);
    }
}