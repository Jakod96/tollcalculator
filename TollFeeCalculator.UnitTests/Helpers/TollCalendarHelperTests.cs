using TollFeeCalculator.Helpers;

namespace TollFeeCalculator.UnitTests.Helpers;

public class TollCalendarHelperTests
{
    [Fact]
    public void IsTollFreeDay_DateInJune_ReturnsTrue()
    {
        DateOnly date = new DateOnly(2025, 6, 1);

        var result = TollCalendarHelper.IsTollFreeDay(date);
        
        Assert.True(result);
    }
    
    [Fact]
    public void IsTollFreeDay_WeekendDate_ReturnsTrue()
    {
        DateOnly date = new DateOnly(2025, 4, 6);

        var result = TollCalendarHelper.IsTollFreeDay(date);
        
        Assert.True(result);
    }

    [Fact]
    public void IsTollFreeDay_RegularWeekDay_ReturnsFalse()
    {
        DateOnly date = new DateOnly(2025, 4, 7);

        var result = TollCalendarHelper.IsTollFreeDay(date);
        
        Assert.False(result);
    }
}