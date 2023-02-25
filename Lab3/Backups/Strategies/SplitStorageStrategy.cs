using Backups.Models;

namespace Backups.Strategies
{
    public class SplitStorageStrategy : IStrategy
    {
        public List<IStorage> Archive(IList<IBackupObject> trackedObjects)
        {
            List<IStorage> result = new List<IStorage>();
            foreach (var backupObject in trackedObjects)
            {
                result.Add(new Storage(new List<IBackupObject> { backupObject }, backupObject.Name));
            }

            return result;
        }
    }
}