using Backups.Models;

namespace Backups.Strategies
{
    public class SingleStorageStrategy : IStrategy
    {
        public List<IStorage> Archive(IList<IBackupObject> trackedObjects)
        {
            List<IStorage> result = new List<IStorage>();
            result.Add(new Storage(trackedObjects, $"backup"));

            return result;
        }
    }
}