using System.Collections.Generic;
using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private List<Group> groupsList = new List<Group>();
    private List<Student> allStudentsList = new List<Student>();

    private int numberOfStudent = 1;

    public List<Student> GetAllStudentsList() => allStudentsList;
    public List<Group> GetAllGroupsList() => groupsList;

    public Group AddGroup(GroupName groupName)
    {
        if (CheckIfGroupNameAlreadyRegistred(groupName))
        {
            throw new GroupNameAlreadyRegistred(groupName.ToString());
        }

        var newGroup = new Group(groupName);
        groupsList.Add(newGroup);

        return newGroup;
    }

    public bool CheckIfGroupNameAlreadyRegistred(GroupName newGroup)
    {
        try
        {
            groupsList.First(group => group.GroupId.Equals(newGroup));
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public Student AddStudent(Group group, string name)
    {
        var newStudent = new Student(name, group, numberOfStudent++);
        group.AddStudent(newStudent);
        allStudentsList.Add(newStudent);

        return newStudent;
    }

    public Student GetStudent(int id)
    {
        foreach (var group in groupsList)
        {
            try
            {
                var rv = group.GetStudentsList().First(student => student.SelfId == id);
                return rv;
            }
            catch (InvalidOperationException)
            {
            }
        }

        throw new StudentNotFound(id);
    }

    public Group? FindGroup(GroupName groupName)
    {
        try
        {
            var rv = groupsList.First(group => group.GroupId.Equals(groupName));
            return rv;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public Student? FindStudent(int id)
    {
        foreach (var group in groupsList)
        {
            try
            {
                var rv = group.GetStudentsList().First(student => student.SelfId == id);
                return rv;
            }
            catch (InvalidOperationException)
            {
            }
        }

        return null;
    }

    public List<Student> FindStudents(string groupName)
    {
        var group = FindGroup(new GroupName(groupName));
        var foundStudents = new List<Student>();
        if (group == null)
        {
            return foundStudents;
        }

        foreach (var student in group.GetStudentsList())
        {
            foundStudents.Add(student);
        }

        return foundStudents;
    }

    public List<Student> FindStudents(int courseNumber)
    {
        var suitableCourseGroups = FindGroups(courseNumber);
        if (suitableCourseGroups == null)
            return new List<Student>();

        List<Student> result = new List<Student>();
        foreach (var group in suitableCourseGroups)
        {
            result.AddRange(group.GetStudentsList());
        }

        return result;
    }

    public List<Group>? FindGroups(int courseNumber)
    {
        List<Group> result = new List<Group>();
        foreach (var group in groupsList)
        {
            if (group.CourseNumber == courseNumber)
            {
                result.Add(group);
            }
        }

        return result.Count == 0 ? null : result;
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        student.ChangeGroup(newGroup);
    }
}