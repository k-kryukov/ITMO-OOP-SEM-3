namespace Backups.Models
{
    public class Storage : IStorage
    {
        private List<IBackupObject> _backupObjects;
        private string _name;

        public Storage(IList<IBackupObject> backupObjects, string name)
        {
            _backupObjects = new List<IBackupObject>(backupObjects);
            _name = name;
        }

        public IList<IBackupObject> BackupObjects { get { return _backupObjects.AsReadOnly(); } }
        public string Name { get { return _name; } }

        public override string ToString() { return _name; }
    }
}