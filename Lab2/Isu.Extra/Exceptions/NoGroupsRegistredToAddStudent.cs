namespace IsuExtra.Exceptions;

public class NoGroupsRegistredToAddStudent : Exception
{
    public NoGroupsRegistredToAddStudent(string studentFullName, string courseName)
    : base($"Unable to add student {studentFullName}: there are no groups in {courseName}") { }
}