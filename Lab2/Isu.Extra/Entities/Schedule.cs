namespace IsuExtra.Entities;

using IsuExtra.Models;
using IsuExtra.Exceptions;

public class Schedule
{
    private List<Lesson> _lessons;
    private IGroup _group;

    public Schedule(IGroup group)
    {
        _group = group;
        _lessons = new List<Lesson>();
    }

    public IList<Lesson> Lessons { get { return _lessons.AsReadOnly(); } }

    public void AddLesson(LessonTime beginTime, Teacher teacher, Subject subject, Room room)
    {
        Lesson newLesson = new Lesson(beginTime, teacher, room, _group, subject);

        Lesson? oldLesson = (from lesson in _lessons
                            where lesson.BeginTime.Equals(newLesson.BeginTime)
                            select lesson).FirstOrDefault();
        if (oldLesson != null)
            throw new TimeslotAlreadyOccupated(oldLesson.BeginTime, oldLesson.Subject);
        else
            _lessons.Add(newLesson);
    }
}