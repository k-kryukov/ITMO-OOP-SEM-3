using Backups.Entities;
using Backups.Exceptions;
using Backups.Models;
using Backups.Strategies;
using Xunit;

namespace Backups.Test;

public class BackupsTest
{
    [Fact]
    public void TestSplitStorageBackup()
    {
        var strategy = new SplitStorageStrategy();
        var rootFolderName = "testBackupFolder";
        var fileSystem = new InMemoryFileSystem(rootFolderName);
        var repository = new Repository(strategy, rootFolderName, fileSystem);
        var task = new BackupTask(strategy, repository);

        var root = fileSystem.Root;
        root.AddChild("file_1", false);
        root.AddChild("file_2", false);

        task.TrackObject("file_1");
        task.TrackObject("file_2");
        task.Execute();

        task.UntrackObject("file_2");
        task.Execute();

        Console.WriteLine(root.GetNextByName("file_1(1)").GetNextByName("file_1(1)"));
        Console.WriteLine(root.GetNextByName("file_2(1)").GetNextByName("file_2(1)"));
        Console.WriteLine(root.GetNextByName("file_1(2)").GetNextByName("file_1(2)"));
        Assert.Throws<PathIsNotCorrect>(() => Console.WriteLine(root.GetNextByName("file_2(2)").GetNextByName("file_2(2)")));
        Assert.True(task.NumberOfRestorePoints == 2);
    }

    [Fact]
    public void TestSingleStorageBackup()
    {
        var strategy = new SingleStorageStrategy();
        var rootFolderName = "testBackupFolder";
        var fileSystem = new InMemoryFileSystem(rootFolderName);
        var repository = new Repository(strategy, rootFolderName, fileSystem);
        var task = new BackupTask(strategy, repository);

        var root = fileSystem.Root;
        root.AddChild("file_1", false);
        root.AddChild("file_2", false);

        task.TrackObject("file_1");
        task.TrackObject("file_2");
        task.Execute();

        Console.WriteLine(root.GetNextByName("backup(1)").GetNextByName("file_1(1)"));
        Console.WriteLine(root.GetNextByName("backup(1)").GetNextByName("file_1(1)"));
    }
}