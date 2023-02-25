namespace IsuExtra.Entities;

using IsuExtra.Models;
using IsuExtra.Exceptions;

public class ExtendedGroupBuilder
{
    private ExtendedGroup _extGroup;

    public ExtendedGroupBuilder(GroupName groupName) { _extGroup = new ExtendedGroup(groupName); }

    public void AddLesson(LessonTime beginTime, Teacher teacher, Subject subject, Room room)
    {
        _extGroup.AddLesson(beginTime, teacher, subject, room);
    }

    public ExtendedGroup Create() { return _extGroup; }
}