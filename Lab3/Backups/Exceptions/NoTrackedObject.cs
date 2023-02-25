namespace Backups.Exceptions;

public class NoTrackedObject : Exception
{
    public NoTrackedObject(string path)
    : base($"Object {path} is not tracked yet!") { }
}