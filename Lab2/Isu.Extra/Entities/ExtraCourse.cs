namespace IsuExtra.Entities;

using IsuExtra.Models;
using IsuExtra.Exceptions;
using System;

public class ExtraCourse
{
    private List<ExtendedGroup> _groups;
    private string _courseName;

    public ExtraCourse(string courseName)
    {
        _groups = new List<ExtendedGroup>();
        _courseName = courseName;
    }

    public string Name { get { return _courseName; } }

    public IList<ExtendedGroup> Groups { get { return _groups; } }

    public ExtendedGroup AddStudent(ExtendedStudent student)
    {
        if (_groups.Count == 0)
            throw new NoGroupsRegistredToAddStudent(student.FullName, _courseName);

        int groupNumber = 0;
        for (; groupNumber < _groups.Count; ++groupNumber)
        {
            if (CheckIfThereAreLessonsIntersections(_groups[groupNumber], (ExtendedGroup)student.StudentGroup))
            {
                if (groupNumber == _groups.Count - 1)
                {
                    throw new IntersectionWithMainSchedule(student.FullName);
                }
            }
            else
            {
                break;
            }
        }

        var studentGroup = _groups[groupNumber];
        studentGroup.AddStudent(student);

        return studentGroup;
    }

    public ExtendedGroup AddGroup(ExtraGroupName groupName)
    {
        ExtendedGroup extraCourseGroup = new ExtendedGroup(groupName);
        _groups.Add(extraCourseGroup);

        return extraCourseGroup;
    }

    public override string ToString() { return $"{_courseName}"; }

    private bool CheckIfThereAreLessonsIntersections(ExtendedGroup group1, ExtendedGroup group2)
    {
        var lessons1 = group1.Lessons;
        var lessons2 = group2.Lessons;

        foreach (var lesson_1 in lessons1)
        {
            foreach (var lesson_2 in lessons2)
            {
                if (lesson_1.Intersects(lesson_2))
                    return true;
            }
        }

        return false;
    }
}