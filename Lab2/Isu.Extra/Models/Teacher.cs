namespace IsuExtra.Models;

public class Teacher
{
    private string _name;

    public Teacher(string name) { _name = name; }

    public string Name { get { return _name; } }

    public override string ToString() { return _name; }
}