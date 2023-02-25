namespace IsuExtra.Entities;

using IsuExtra.Models;
using IsuExtra.Exceptions;

public class ExtendedGroup : Group, IExtendedGroup
{
    private Schedule _schedule;

    public ExtendedGroup(IGroupName groupName)
    : base(groupName)
    {
        _schedule = new Schedule(this);
    }

    public IList<Lesson> Lessons { get { return _schedule.Lessons; } }

    public void AddLesson(LessonTime beginTime, Teacher teacher, Subject subject, Room room)
    {
        _schedule.AddLesson(beginTime, teacher, subject, room);
    }

    public void DisplaySchedule()
    {
        foreach (var lesson in _schedule.Lessons.Select((value, index) => new { value, index }))
        {
            Console.WriteLine($"{lesson.index + 1}: {lesson.value}");
        }
    }
}