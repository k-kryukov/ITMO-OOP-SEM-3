namespace IsuExtra.Exceptions;

public class WrongLessonTime : Exception
{
    public WrongLessonTime(decimal hours, decimal minutes, string weekDay)
    : base($"Lesson time {hours}:{minutes} on {weekDay} is incorrect!") { }
}