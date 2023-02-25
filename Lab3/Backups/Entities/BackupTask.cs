using Backups.Exceptions;
using Backups.Models;
using Backups.Strategies;

namespace Backups.Entities
{
    public class BackupTask
    {
        private IRepository _repository;
        private List<IBackupObject> _backupObjects;
        private IBackup _backup;

        public BackupTask(IStrategy strategy, IRepository repository)
        {
            _repository = repository;
            _backupObjects = new List<IBackupObject>();
            _backup = new Backup();
        }

        public int NumberOfRestorePoints { get { return _backup.GetRestoreNumber(); } }

        public IList<IBackupObject> BackupObjects { get { return _backupObjects.AsReadOnly(); } }
        public void TrackObject(string path)
        {
            var objectName = _repository.GetObjectNameByPath(path);
            if (objectName == null)
                throw new PathIsNotCorrect(path);

            var insertingObject = (from backupObject in _backupObjects
                                where backupObject.Path == path
                                select backupObject).FirstOrDefault();
            if (insertingObject == null)
                _backupObjects.Add(new BackupObject(path, objectName));
        }

        public void UntrackObject(string path)
        {
            var desiredObject = (from backupObject in _backupObjects
                                where backupObject.Path == path
                                select backupObject).FirstOrDefault();

            if (desiredObject == null)
            {
                throw new NoTrackedObject(path);
            }
            else
            {
                _backupObjects.Remove(desiredObject);
            }
        }

        public void Execute()
        {
            var restorePoint = new RestorePoint(_backupObjects.AsReadOnly());
            _backup.AddRestorePoint(restorePoint);

            _repository.Restore(_backupObjects.AsReadOnly(), _backup.GetRestoreNumber());
        }
    }
}