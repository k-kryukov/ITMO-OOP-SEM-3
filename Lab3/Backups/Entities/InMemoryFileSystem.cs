using System.IO;
using Backups.Exceptions;
using Backups.Models;

namespace Backups.Entities
{
    public class InMemoryFileSystem : IFileSystem
    {
        private Node root;

        public InMemoryFileSystem(string name)
        {
            root = new Node(name, true);
        }

        public INode Root { get { return root; } }

        public INode AddChild(string name, bool isDirectory) { return root.AddChild(name, isDirectory); }

        public string GetObjectNameByPath(string path)
        {
            return path;
        }

        public INode GetRootChildByName(string name)
        {
            return root.GetNextByName(name);
        }

        public void Restore(IList<IStorage> storages, string backupFolder, int backupNumber)
        {
            foreach (var storage in storages)
            {
                var backupFolderNode = root.AddChild($"{storage.Name}({backupNumber})", true);

                foreach (var backupObject in storage.BackupObjects)

                    backupFolderNode.AddChild($"{backupObject.Name}({backupNumber})", false);
            }
        }
    }
}