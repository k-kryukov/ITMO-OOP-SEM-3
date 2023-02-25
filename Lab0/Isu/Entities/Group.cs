namespace Isu.Entities;

using Isu.Models;
using Isu.Exceptions;

public class Group : IEquatable<Group>
{
    private List<Student> studentsList;

    public Group(GroupName groupName)
    {
        GroupId = groupName;
        studentsList = new List<Student>();
        CourseNumber = ExtractCourseNumber(GroupId);
    }

    public int MaxStudentsNumberInGroup { get; } = 25;

    public GroupName GroupId { get; private set; }

    public int CourseNumber { get; private set; }

    public int NumberOfStudents { get { return studentsList.Count; } }

    public IEnumerable<Student> GetStudentsList()
    {
        foreach (var x in studentsList)
        {
            yield return x;
        }
    }

    public void AddStudent(Student student)
    {
        if (NumberOfStudents >= MaxStudentsNumberInGroup)
        {
            throw new MaxStudentsNumberPerGroupReached(student.FullName);
        }

        studentsList.Add(student);
    }

    public void DeleteStudent(Student student)
    {
        if (studentsList.Contains(student))
        {
            studentsList.Remove(student);
            return;
        }

        throw new NoRequiredStudentInGroup(student.SelfId, GroupId.ToString());
    }

    public override string ToString() => $"Group number {GroupId}";

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        Group? objAsGroup = obj as Group;
        if (objAsGroup == null)
            return false;
        else
            return Equals(objAsGroup);
    }

    public override int GetHashCode()
    {
        return GroupId.GetHashCode();
    }

    public bool Equals(Group? other)
    {
        if (other == null)
            return false;
        return this.GroupId == other.GroupId;
    }

    private static int ExtractCourseNumber(GroupName groupId)
    {
        return Convert.ToInt32(groupId.ToString()[2]);
    }
}