using System.IO;
using Backups.Models;

namespace Backups.Entities
{
    public interface IFileSystem
    {
        void Restore(IList<IStorage> storages, string backupFolder, int backupNumber);
        string GetObjectNameByPath(string path);
    }
}