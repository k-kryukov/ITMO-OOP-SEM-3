using Backups.Models;

namespace Backups.Entities
{
    public interface IBackup
    {
        void AddRestorePoint(IRestorePoint restorePoint);
        int GetRestoreNumber();
    }
}