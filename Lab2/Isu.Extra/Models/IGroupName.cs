namespace IsuExtra.Models;

using IsuExtra.Exceptions;

public interface IGroupName
{
    string Name { get; }
    string ToString();

    bool Equals(object? obj);
    int GetHashCode();
    bool Equals(IGroupName? other);
}