namespace IsuExtra.Exceptions;

public class MaxStudentsNumberPerGroupReached : Exception
{
    public MaxStudentsNumberPerGroupReached(string studentFullName)
    : base($"Unable to add student {studentFullName}: group is overcrowded") { }
}