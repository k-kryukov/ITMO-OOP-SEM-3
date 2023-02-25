using System.IO;
using Backups.Exceptions;
using Backups.Models;

namespace Backups.Entities
{
    public class FileSystem : IFileSystem
    {
        public string GetObjectNameByPath(string path)
        {
            string name;

            if (File.Exists(path))
                name = (new FileInfo(path)).Name;
            else if (Directory.Exists(path))
                name = (new DirectoryInfo(path)).Name;
            else
                throw new PathIsNotCorrect(path);

            return name;
        }

        public void Restore(IList<IStorage> storages, string backupFolder, int backupNumber)
        {
            foreach (var storage in storages)
            {
                Directory.CreateDirectory($"{backupFolder}/{storage.Name}({backupNumber})");

                foreach (var backupObject in storage.BackupObjects)
                {
                    if (File.Exists(backupObject.Path))
                        File.Copy(backupObject.Path, $"{backupFolder}/{storage.Name}({backupNumber})/{backupObject.Name}({backupNumber})");
                    else
                        CopyDirectory(backupObject.Path, $"{backupFolder}/{storage.Name}({backupNumber})/{backupObject.Name}({backupNumber})");
                }
            }
        }

        protected void CopyDirectory(string sourceDir, string destinationDir)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destinationDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir);
            }
        }
    }
}