namespace IsuExtra.Entities;

using IsuExtra.Models;
using IsuExtra.Exceptions;

public class Group : IEquatable<IGroup>, IGroup
{
    private List<IStudent> studentsList;

    public Group(IGroupName groupName)
    {
        GroupId = groupName;
        studentsList = new List<IStudent>();
        CourseNumber = ExtractCourseNumber(GroupId);
    }

    public int MaxStudentsNumberInGroup { get; } = 25;

    public IGroupName GroupId { get; private set; }

    public int CourseNumber { get; private set; }

    public int NumberOfStudents { get { return studentsList.Count; } }

    public IEnumerable<IStudent> GetStudentsList()
    {
        foreach (var x in studentsList)
        {
            yield return x;
        }
    }

    public void AddStudent(IStudent student)
    {
        if (NumberOfStudents >= MaxStudentsNumberInGroup)
        {
            throw new MaxStudentsNumberPerGroupReached(student.FullName);
        }

        studentsList.Add(student);
    }

    public void DeleteStudent(IStudent student)
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

    public bool Equals(IGroup? other)
    {
        if (other == null)
            return false;
        return this.GroupId == other.GroupId;
    }

    private static int ExtractCourseNumber(IGroupName groupId)
    {
        return Convert.ToInt32(groupId.ToString()[2]);
    }
}