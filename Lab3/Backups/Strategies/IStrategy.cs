using Backups.Models;

namespace Backups.Strategies
{
    public interface IStrategy
    {
        List<IStorage> Archive(IList<IBackupObject> trackedObjects);
    }
}