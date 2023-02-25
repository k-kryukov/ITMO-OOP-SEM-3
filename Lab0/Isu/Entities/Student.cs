namespace Isu.Entities;

using Isu.Models;

public class Student : IEquatable<Student>
{
    public Student(string fullName, Group group, int id)
    {
        FullName = fullName;
        StudentGroup = group;
        SelfId = id;
    }

    public string FullName { get; }

    public int SelfId { get; }

    public Group StudentGroup { get; private set; }

    public int CourseNumber { get; private set; }

    public override string ToString()
    {
        return $"Student {FullName} with ISU number {SelfId} ({StudentGroup})";
    }

    public void ChangeGroup(Group newGroup)
    {
        newGroup.AddStudent(this);
        StudentGroup.DeleteStudent(this); // won't be executed if AddStudent fails
        StudentGroup = newGroup;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        Student? objAsStudent = obj as Student;
        if (objAsStudent == null)
            return false;
        else
            return Equals(objAsStudent);
    }

    public override int GetHashCode()
    {
        return SelfId;
    }

    public bool Equals(Student? other)
    {
        if (other == null)
            return false;
        return SelfId.Equals(other.SelfId);
    }
}