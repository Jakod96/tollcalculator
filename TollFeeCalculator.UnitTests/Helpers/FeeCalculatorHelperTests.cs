using TollFeeCalculator.Enums;
using TollFeeCalculator.Helpers;

namespace TollFeeCalculator.UnitTests.Helpers;

public class FeeCalculatorHelperTests
{
    [Fact]
    public void GetTollFeeForPeriod_NoFeeAtNight_ReturnsZero()
    {
        List<DateTime> dateTimes =
        [
            DateTime.Parse("2024-03-04T05:00"), // A Monday.
        ];
        Assert.Equal(0, FeeCalculatorHelper.GetTollFeeForPeriod(dateTimes));
    }
    
    [Theory]
    [InlineData(VehicleType.Diplomat)]
    [InlineData(VehicleType.Emergency)]
    [InlineData(VehicleType.Foreign)]
    [InlineData(VehicleType.Military)]
    [InlineData(VehicleType.Motorbike)]
    [InlineData(VehicleType.Tractor)]
    public void IsTollFreeVehicle_TollFreeVehicleType_ReturnsTrue(VehicleType vehicleType)
    {
        Assert.True(FeeCalculatorHelper.IsTollFreeVehicle(vehicleType));
    }
    
    [Theory]
    [InlineData(VehicleType.Car)]
    public void IsTollFreeVehicle_NotTollFreeVehicleType_ReturnsFalse(VehicleType vehicleType)
    {
        Assert.False(FeeCalculatorHelper.IsTollFreeVehicle(vehicleType));
    }
}