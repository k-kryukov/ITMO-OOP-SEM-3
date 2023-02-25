namespace IsuExtra.Entities;

using IsuExtra.Models;
using IsuExtra.Exceptions;

public interface IExtendedGroup : IGroup
{
    void AddLesson(LessonTime beginTime, Teacher teacher, Subject subject, Room room);

    void DisplaySchedule();
}