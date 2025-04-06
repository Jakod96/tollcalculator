namespace TollFeeCalculator.Helpers;

//Naming these helpers is a bit... eh
public static class TollCalendarHelper
{
    /// <summary>
    /// Checks if a given day is one where there is no toll fee.
    /// </summary>
    /// <param name="date">The day to check.</param>
    /// <returns><c>true</c> if <c>date</c> is a toll-free day.</returns>
    public static bool IsTollFreeDay(DateOnly date)
    {
        // Weekends are toll-free.
        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            return true;
        }
        
        // June is toll-free.
        if (date.Month == 6)
        {
            return true;
        }

        // Holidays and days before holidays are toll-free.
        return GetHolidays().Contains(date) || GetHolidays().Contains(date.AddDays(1));
    }

    // Hardcoding these dates is bad. Ideally you'd get these from a database or an external provider
    // IHolidayRepository.GetHolidays(int year); etc
    // These are based on https://kalender.se/helgdagar
    private static readonly List<string> HolidayDates =
    [
        "2025-01-01",
        "2025-01-06",
        "2025-04-18",
        "2025-04-20",
        "2025-04-21",
        "2025-05-01",
        "2025-05-29",
        "2025-06-06",
        "2025-06-08",
        "2025-06-21",
        "2025-11-01",
        "2025-12-25",
        "2025-12-26"
    ];

    private static HashSet<DateOnly> GetHolidays()
    {
        return new(HolidayDates.Select(DateOnly.Parse));
    } 
}