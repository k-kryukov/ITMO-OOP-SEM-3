namespace IsuExtra.Models;

using IsuExtra.Exceptions;

public class GroupName : IEquatable<IGroupName>, IGroupName
{
    private static string groupNamePattern = @"[A-Z]\d[1-4]\d\d\d";

    public GroupName(string groupName)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(groupName.ToString(), groupNamePattern))
        {
            throw new InvalidGroupName(groupName);
        }

        Name = groupName;
    }

    public string Name { get; }

    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        GroupName? objAsGroupName = obj as GroupName;
        if (objAsGroupName == null)
            return false;
        else
            return Equals(objAsGroupName);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public bool Equals(IGroupName? other)
    {
        if (other == null)
            return false;

        return Name.Equals(other.Name);
    }
}