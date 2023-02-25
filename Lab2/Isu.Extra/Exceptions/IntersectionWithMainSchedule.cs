namespace IsuExtra.Exceptions;

public class IntersectionWithMainSchedule : Exception
{
    public IntersectionWithMainSchedule(string studentFullName)
    : base($"Unable to add student {studentFullName} to extraCourse") { }
}