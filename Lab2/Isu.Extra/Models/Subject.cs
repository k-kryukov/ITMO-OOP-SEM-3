namespace IsuExtra.Models;

public class Subject
{
    private string _name;

    public Subject(string name) { _name = name; }

    public string Name { get { return _name; } }

    public override string ToString() { return _name; }
}