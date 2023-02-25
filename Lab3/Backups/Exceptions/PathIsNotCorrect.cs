namespace Backups.Exceptions;

public class PathIsNotCorrect : Exception
{
    public PathIsNotCorrect(string path)
    : base($"Path {path} is not correct!") { }
}