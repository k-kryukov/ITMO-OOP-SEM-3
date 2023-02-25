using Backups.Exceptions;

namespace Backups.Models
{
    public class Node : INode
    {
        private List<INode> _children;
        private string _name;
        private bool _isDirectory;

        public Node(string name, bool isDirectory)
        {
            _children = new List<INode>();
            _name = name;
            _isDirectory = isDirectory;
        }

        public string Name { get { return _name; } }
        public bool IsDirectory { get { return _isDirectory; } }
        public IList<INode> Children { get { return _children.AsReadOnly(); } }

        public INode AddChild(string name, bool isDirectory)
        {
            if (!_isDirectory)
                throw new CannotAddChildToFile(_name);

            INode child = new Node(name, isDirectory);
            _children.Add(child);

            return child;
        }

        public INode GetNextByName(string name)
        {
            INode? desired = (from node in _children
                                where node.Name == name
                                select node).FirstOrDefault();
            if (desired == null)
                throw new PathIsNotCorrect(name);

            return desired;
        }

        public bool FileExists(string name)
        {
            INode? desired = (from node in _children
                                where node.Name == name && !node.IsDirectory
                                select node).FirstOrDefault();
            return desired == null;
        }

        public bool DirectoryExists(string name)
        {
            INode? desired = (from node in _children
                                where node.Name == name && node.IsDirectory
                                select node).FirstOrDefault();
            return desired == null;
        }

        public void PrintAllChildren()
        {
            foreach (var node in _children)
            {
                Console.WriteLine(node);
            }
        }

        public override string ToString() { return _name; }
    }
}