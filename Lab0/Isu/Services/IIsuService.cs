using Isu.Entities;
using Isu.Models;

namespace Isu.Services;

public interface IIsuService
{
    Group AddGroup(GroupName groupName);

    Student AddStudent(Group group, string name);
    Student GetStudent(int id);
    Student? FindStudent(int id);

    List<Student> FindStudents(string groupName);
    List<Student> FindStudents(int courseNumber);
    Group? FindGroup(GroupName groupName);
    List<Group>? FindGroups(int courseNumber);
    void ChangeStudentGroup(Student student, Group newGroup);
}
