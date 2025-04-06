using TollFeeCalculator.Enums;

namespace TollFeeCalculator.UnitTests;

public class TollFeeCalculatorTests
{
    private readonly TollCalculator _tollFeeCalculator = new();

    [Fact]
    public void GetTollFee_NoEvents_ReturnsZero()
    {
        List<DateTime> dateTimes = [];
        Assert.Equal(0, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void GetTollFee_OneEvent_ReturnsFee()
    {
        List<DateTime> dateTimes = [DateTime.Parse("2025-04-01T06:45")]; // A Monday - 13kr.

        Assert.Equal(13, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void GetTollFee_MultipleEventsInSameFeePeriod_ReturnsFeeForOneEvent()
    {
        List<DateTime> dateTimes =
        [
            DateTime.Parse("2025-04-01T15:45"), // A Monday - 18kr.
            DateTime.Parse("2025-04-01T15:50")  // A Monday - 18kr
        ];

        Assert.Equal(18, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void GetTollFee_MultipleEventsInSamePeriod_ReturnsHighestFee()
    {
        List<DateTime> dateTimes =
        [
            DateTime.Parse("2025-04-01T14:45"), // A Monday - 8kr
            DateTime.Parse("2025-04-01T15:15")  // A Monday - 13kr
        ];
        Assert.Equal(13, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void GetTollFee_MultipleEventsInDifferentPeriods_ReturnsHighestFee()
    {
        List<DateTime> dateTimes =
        [
            DateTime.Parse("2025-04-01T08:15"), // A Monday - 13kr
            DateTime.Parse("2025-04-01T15:45"), // A Monday. - 18kr
            DateTime.Parse("2025-04-01T18:50"), // A monday - 0kr
        ];
        Assert.Equal(31, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void GetTollFee_MultipleEventsInDifferentPeriodsMoreThanMaxFee_ReturnsMaxFee()
    {
        List<DateTime> dateTimes =
        [
            DateTime.Parse("2025-04-01T06:05"), // A Monday.
            DateTime.Parse("2025-04-01T07:10"),
            DateTime.Parse("2025-04-01T08:15"),
            DateTime.Parse("2025-04-01T09:20"),
            DateTime.Parse("2025-04-01T10:25"),
            DateTime.Parse("2025-04-01T11:30"),
            DateTime.Parse("2025-04-01T12:35"),
            DateTime.Parse("2025-04-01T13:40"),
            DateTime.Parse("2025-04-01T14:45"),
            DateTime.Parse("2025-04-01T15:50"),
            DateTime.Parse("2025-04-01T16:55"),
            DateTime.Parse("2025-04-01T18:00")
        ];
        Assert.Equal(60, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }


    [Fact]
    public void GetTollFee_NoFeeAtNight_ReturnsZero()
    {
        List<DateTime> dateTimes =
        [
            DateTime.Parse("2024-03-04T05:00"), // A Monday.
        ];
        Assert.Equal(0, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }
}