namespace IsuExtra.Entities;

using IsuExtra.Models;
using IsuExtra.Exceptions;
using System;

public class Lesson
{
    private LessonTime _beginTime;
    private Teacher _teacher;
    private Room _room;
    private IGroup _group;
    private Subject _subject;

    public Lesson(LessonTime beginTime, Teacher teacher, Room room, IGroup group, Subject subject)
    {
        _beginTime = beginTime;
        _teacher = teacher;
        _room = room;
        _group = group;
        _subject = subject;
    }

    public static uint Duration { get; } = 90;

    public LessonTime BeginTime { get { return _beginTime; } }
    public Teacher Teacher { get { return _teacher; } }
    public Room Room { get { return _room; } }
    public IGroup Group { get { return _group; } }
    public Subject Subject { get { return _subject; } }

    public void ChangeTeacher(Teacher newTeacher) { _teacher = newTeacher; }
    public void ChangeRoom(Room newRoom) { _room = newRoom; }
    public override string ToString() { return $"{_subject.Name} in {_room.Number} ({_beginTime} with {_teacher.Name})"; }
    public bool Intersects(Lesson other) { return this._beginTime.Equals(other._beginTime); }
}