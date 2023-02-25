namespace Backups.Models
{
    public interface IStorage
    {
        IList<IBackupObject> BackupObjects { get; }
        string Name { get; }

        string ToString();
    }
}