using TollFeeCalculator.Enums;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Helpers;

//Naming these helpers is a bit... eh
public static class FeeCalculatorHelper
{
    public static bool IsTollFreeVehicle(VehicleType vehicleType)
    {
        return GetTollFreeVehicles().Contains(vehicleType);
    }

    public static decimal GetTollFeeForPeriod(IEnumerable<DateTime> dateTimes)
    {
        return dateTimes.Select(GetTollFeeForSingleEvent).Max();
    }

    private static List<FeeTime> GetFeeTimes()
    {
        return
        [
            new(TimeOnly.MinValue, 0),
            new(new TimeOnly(6, 0), 8),
            new(new TimeOnly(6, 30), 13),
            new(new TimeOnly(7, 0), 18),
            new(new TimeOnly(8, 0), 13),
            new(new TimeOnly(8, 30), 8),
            new(new TimeOnly(15, 0), 13),
            new(new TimeOnly(15, 30), 18),
            new(new TimeOnly(17, 0), 13),
            new(new TimeOnly(18, 0), 8),
            new(new TimeOnly(18, 30), 0)
        ];

    }

    private static HashSet<VehicleType> GetTollFreeVehicles()
    {
        return
        [
            VehicleType.Diplomat,
            VehicleType.Emergency,
            VehicleType.Foreign,
            VehicleType.Military,
            VehicleType.Motorbike,
            VehicleType.Tractor
        ];
    }

    private static decimal GetTollFeeForSingleEvent(DateTime dateTime)
    {
        var eventTime = TimeOnly.FromDateTime(dateTime);
        var eventList = GetFeeTimes();
        eventList.Reverse();
        return eventList.First(feeTime => eventTime >= feeTime.Time).Fee;
    }
}