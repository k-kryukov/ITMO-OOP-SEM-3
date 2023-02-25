using Backups.Exceptions;
using Backups.Models;
using Backups.Strategies;

namespace Backups.Entities
{
    public interface IRepository
    {
        string GetObjectNameByPath(string path);

        void Restore(IList<IBackupObject> trackedObjects, int restorePointNumber);
    }
}