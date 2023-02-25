namespace IsuExtra.Exceptions;

public class InvalidGroupName : Exception
{
    public InvalidGroupName(string groupName)
    : base($"Group name {groupName} doesn't match pattern!") { }
}