using Backups.Models;

namespace Backups.Entities
{
    public class Backup : IBackup
    {
        private List<IRestorePoint> _restorePoints;

        public Backup() { _restorePoints = new List<IRestorePoint>(); }

        public void AddRestorePoint(IRestorePoint restorePoint)
        {
            _restorePoints.Add(restorePoint);
        }

        public int GetRestoreNumber() { return _restorePoints.Count; }
    }
}