namespace Backups.Models
{
    public class BackupObject : IBackupObject
    {
        private string _path;
        private string _name;

        public BackupObject(string path, string name)
        {
            _path = path;
            _name = name;
        }

        public string Path { get { return _path; } }
        public string Name { get { return _name; } }

        public override string ToString() { return _path; }
    }
}