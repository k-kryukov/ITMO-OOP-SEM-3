using System.IO;

using Backups.Entities;
using Backups.Strategies;

namespace A
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var strategy = new SingleStorageStrategy();
            var fileSystem = new FileSystem();
            var repository = new Repository(strategy, "backupsFolder", fileSystem);
            var task = new BackupTask(strategy, repository);

            task.TrackObject("file_1");
            task.TrackObject("file_2");
            task.Execute();

            task.UntrackObject("file_1");
            task.Execute();
        }
    }
}