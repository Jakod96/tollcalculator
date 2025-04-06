using TollFeeCalculator.Enums;

namespace TollFeeCalculator.UnitTests;

public class TollFeeCalculatorTests
{
    private readonly TollCalculator _tollFeeCalculator;
    public TollFeeCalculatorTests()
    {
        _tollFeeCalculator = new TollCalculator();
    }
    
    [Fact]
    public void NoEvents()
    {
        List<DateTime> dateTimes = [];
        Assert.Equal(0, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void OneEvent()
    {
        List<DateTime> dateTimes = [DateTime.Parse("2024-03-04T06:45")]; // A Monday.

        Assert.Equal(13, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void SeveralEventsInOnePeriod()
    {
        List<DateTime> dateTimes =
        [
            DateTime.Parse("2024-03-04T15:45"), // A Monday.
            DateTime.Parse("2024-03-04T15:40"),
            DateTime.Parse("2024-03-04T15:50")
        ];

        Assert.Equal(18, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void SeveralEventsInOnePeriodGetsHighestFee()
    {
        List<DateTime> dateTimes =
        [
            DateTime.Parse("2024-03-04T14:45"), // A Monday.
            DateTime.Parse("2024-03-04T15:15")
        ];
        Assert.Equal(13, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void SeveralEvents()
    {
        List<DateTime> dateTimes =
        [
            DateTime.Parse("2024-03-04T15:45"), // A Monday.
            DateTime.Parse("2024-03-04T15:40"),
            DateTime.Parse("2024-03-04T15:50"),
            DateTime.Parse("2024-03-04T18:50"),
            DateTime.Parse("2024-03-04T08:15"),
            DateTime.Parse("2024-03-04T08:16")
        ];
        Assert.Equal(31, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void NotMoreThanMaximum()
    {
        List<DateTime> dateTimes =
        [
            DateTime.Parse("2024-03-04T06:05"), // A Monday.
            DateTime.Parse("2024-03-04T07:10"),
            DateTime.Parse("2024-03-04T08:15"),
            DateTime.Parse("2024-03-04T09:20"),
            DateTime.Parse("2024-03-04T10:25"),
            DateTime.Parse("2024-03-04T11:30"),
            DateTime.Parse("2024-03-04T12:35"),
            DateTime.Parse("2024-03-04T13:40"),
            DateTime.Parse("2024-03-04T14:45"),
            DateTime.Parse("2024-03-04T15:50"),
            DateTime.Parse("2024-03-04T16:55"),
            DateTime.Parse("2024-03-04T18:00")
        ];
        Assert.Equal(60, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void PeriodEdgeCases()
    {
        var dateTime = DateTime.Parse("2024-03-04T15:45");
        List<DateTime> dateTimes =
        [
            dateTime,
            dateTime + TimeSpan.FromHours(1)
        ];

        Assert.Equal(36, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));

        dateTimes[1] = dateTimes[1].AddTicks(-1);

        Assert.Equal(18, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void NoFeeAtNight()
    {
        DateTime[] dateTimes =
        [
            DateTime.Parse("2024-03-04T05:00"), // A Monday.
            DateTime.Parse("2024-03-04T18:35")
        ];
        Assert.Equal(0, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void NoFeeInJune()
    {
        List<DateTime> dateTimes = [DateTime.Parse("2024-06-10T15:45")];
        Assert.Equal(0, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }

    [Fact]
    public void NoFeeOnWeekends()
    {
        List<DateTime> dateTimes = [DateTime.Parse("2024-03-09T15:45")]; // A Saturday.
        Assert.Equal(0, _tollFeeCalculator.GetTollFee(VehicleType.Car, dateTimes));
    }
}