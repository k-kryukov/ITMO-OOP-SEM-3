namespace Backups.Exceptions;

public class CannotAddChildToFile : Exception
{
    public CannotAddChildToFile(string name)
    : base($"Cannot add child to a file {name} (not a directory!)") { }
}