namespace IsuExtra.Entities;

using IsuExtra.Models;
using IsuExtra.Exceptions;

public interface IGroup
{
    int MaxStudentsNumberInGroup { get; }
    IGroupName GroupId { get; }
    int CourseNumber { get; }

    IEnumerable<IStudent> GetStudentsList();
    void DeleteStudent(IStudent student);
    string ToString() => $"Group number {GroupId}";
    bool Equals(object? obj);
    int GetHashCode();
    void AddStudent(IStudent student);
}