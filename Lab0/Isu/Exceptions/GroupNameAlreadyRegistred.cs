namespace Isu.Exceptions;

public class GroupNameAlreadyRegistred : Exception
{
    public GroupNameAlreadyRegistred(string groupName)
    : base($"Group name {groupName} is already registred!") { }
}