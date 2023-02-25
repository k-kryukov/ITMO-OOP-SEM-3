using Backups.Exceptions;

namespace Backups.Models
{
    public interface INode
    {
        string Name { get; }
        bool IsDirectory { get; }
        IList<INode> Children { get; }

        INode AddChild(string name, bool isDirectory);

        INode GetNextByName(string name);

        bool FileExists(string name);

        bool DirectoryExists(string name);

        void PrintAllChildren();

        string ToString();
    }
}