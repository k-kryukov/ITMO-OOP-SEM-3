namespace Backups.Models
{
    public interface IBackupObject
    {
        string Path { get; }
        string Name { get; }

        string ToString();
    }
}