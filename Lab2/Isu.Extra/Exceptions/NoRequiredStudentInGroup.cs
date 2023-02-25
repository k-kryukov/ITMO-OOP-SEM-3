namespace IsuExtra.Exceptions;

public class NoRequiredStudentInGroup : Exception
{
    public NoRequiredStudentInGroup(int studentSelfId, string groupId)
    : base($"Student {studentSelfId} doesn't participate in {groupId}") { }
}