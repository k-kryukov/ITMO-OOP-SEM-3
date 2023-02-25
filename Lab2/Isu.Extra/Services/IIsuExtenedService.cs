using IsuExtra.Entities;
using IsuExtra.Models;

namespace IsuExtra.Services;

public interface IIsuExtendedService
{
    ExtendedGroup AddGroup(GroupName groupName);
    void AddLesson(IExtendedGroup group, LessonTime beginTime, Teacher teacher, Subject subject, Room room);

    ExtendedStudent AddStudent(ExtendedGroup group, string name);
    ExtendedStudent GetStudent(int id);
    ExtendedStudent? FindStudent(int id);

    List<IStudent> FindStudents(IGroupName groupName);
    List<IStudent> FindStudents(int courseNumber);
    ExtendedGroup? FindGroup(IGroupName groupName);
    List<IGroup>? FindGroups(int courseNumber);
    void ChangeStudentGroup(ExtendedStudent student, ExtendedGroup newGroup);
}
