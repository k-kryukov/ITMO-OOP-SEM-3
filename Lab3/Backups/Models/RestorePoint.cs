using System;

namespace Backups.Models
{
    public class RestorePoint : IRestorePoint
    {
        private DateTime _date;
        private IList<IBackupObject> _savedObjects;

        public RestorePoint(IList<IBackupObject> savedObjects)
        {
            _date = DateTime.Now;
            _savedObjects = new List<IBackupObject>(savedObjects);
        }
    }
}