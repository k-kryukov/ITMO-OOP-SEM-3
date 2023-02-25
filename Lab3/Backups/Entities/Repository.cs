using Backups.Exceptions;
using Backups.Models;
using Backups.Strategies;

namespace Backups.Entities
{
    public class Repository : IRepository
    {
        private IFileSystem _fileSystem;
        private IStrategy _strategy;
        private string _backupsFolder;

        public Repository(IStrategy strategy, string backupsFolder, IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
            _backupsFolder = backupsFolder;
            _strategy = strategy;
        }

        public string GetObjectNameByPath(string path) { return _fileSystem.GetObjectNameByPath(path); }

        public void Restore(IList<IBackupObject> trackedObjects, int restorePointNumber)
        {
            var curBackupFolderName = string.Format(_backupsFolder, restorePointNumber);

            List<IStorage> storages = _strategy.Archive(trackedObjects);

            _fileSystem.Restore(storages, curBackupFolderName, restorePointNumber);
        }
    }
}