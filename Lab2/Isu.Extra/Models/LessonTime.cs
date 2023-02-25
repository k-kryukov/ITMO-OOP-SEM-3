namespace IsuExtra.Models;

using IsuExtra.Exceptions;

public enum WeekDays : byte
{
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
    Sunday = 7,
}

public class LessonTime
{
    private static HashSet<string> dayOfWeeks = new HashSet<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

    private decimal _hours;
    private decimal _minutes;
    private string _weekDay; // 1:7

    public LessonTime(decimal hours, decimal minutes, string weekDay)
    {
        if (hours >= 24 || minutes >= 60 || !dayOfWeeks.Contains(weekDay))
            throw new WrongLessonTime(hours, minutes, weekDay);

        _hours = hours;
        _minutes = minutes;
        _weekDay = weekDay;
    }

    public decimal Hours { get { return _hours; } }
    public decimal Minutes { get { return _minutes; } }
    public string WeekDay { get { return _weekDay; } }

    public bool Equals(LessonTime other) { return _hours == other._hours && _minutes == other._minutes && _weekDay == other._weekDay; }
    public override string ToString() { return $"{_hours}:{_minutes} on {_weekDay}"; }
}