using System.Collections.Generic;
using IsuExtra.Entities;
using IsuExtra.Exceptions;
using IsuExtra.Models;

namespace IsuExtra.Services;

public class IsuExtraService : IIsuExtendedService
{
    private List<IGroup> groupsList = new List<IGroup>();
    private List<IStudent> allStudentsList = new List<IStudent>();
    private List<ExtraCourse> extraCourseList = new List<ExtraCourse>();

    private int numberOfStudent = 1;

    public List<IStudent> GetAllStudentsList() => allStudentsList;
    public List<IGroup> GetAllGroupsList() => groupsList;

    public ExtendedGroup AddGroup(GroupName groupName)
    {
        if (CheckIfGroupNameAlreadyRegistred(groupName))
        {
            throw new GroupNameAlreadyRegistred(groupName.ToString());
        }

        var newGroup = new ExtendedGroup(groupName);
        groupsList.Add(newGroup);

        return newGroup;
    }

    public ExtraCourse AddNewExtraCourse(string courseName)
    {
        ExtraCourse exCourse = new ExtraCourse(courseName);
        extraCourseList.Add(exCourse);

        return exCourse;
    }

    public ExtendedGroup AddNewExtraCourseGroup(ExtraCourse extraCourse, ExtraGroupName groupName)
    {
        ExtendedGroup newGroup = extraCourse.AddGroup(groupName);
        return newGroup;
    }

    public void AddLesson(IExtendedGroup group, LessonTime beginTime, Teacher teacher, Subject subject, Room room)
    {
        group.AddLesson(beginTime, teacher, subject, room);
    }

    public void AssignOnlineExtraCourseToStudent(ExtraCourse extraCourse, ExtendedStudent student)
    {
        ExtendedGroup newStudentGroup = extraCourse.AddStudent(student);
        student.ChangeOnlineExtraCourseGroup(newStudentGroup);
    }

    public void AssignOfflineExtraCourseToStudent(ExtraCourse extraCourse, ExtendedStudent student)
    {
        ExtendedGroup newStudentGroup = extraCourse.AddStudent(student);
        student.ChangeOfflineExtraCourseGroup(newStudentGroup);
    }

    public void RemoveStudentFromOnlineExtraCourse(ExtendedStudent student)
    {
        var previousGroup = student.RemoveFromOnlineExtraCourse();
        previousGroup.DeleteStudent(student);
    }

    public void RemoveStudentFromOfflineExtraCourse(ExtendedStudent student)
    {
        var previousGroup = student.RemoveFromOfflineExtraCourse();
        previousGroup.DeleteStudent(student);
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

    public ExtendedStudent AddStudent(ExtendedGroup group, string name)
    {
        var newStudent = new ExtendedStudent(name, group, numberOfStudent++);
        group.AddStudent(newStudent);
        allStudentsList.Add(newStudent);

        return newStudent;
    }

    public ExtendedStudent GetStudent(int id)
    {
        foreach (var group in groupsList)
        {
            try
            {
                var rv = group.GetStudentsList().First(student => student.SelfId == id);
                return (ExtendedStudent)rv;
            }
            catch (InvalidOperationException)
            {
            }
        }

        throw new StudentNotFound(id);
    }

    public ExtendedGroup? FindGroup(IGroupName groupName)
    {
        try
        {
            var rv = groupsList.First(group => group.GroupId.Equals(groupName));
            return (ExtendedGroup)rv;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public ExtendedStudent? FindStudent(int id)
    {
        foreach (var group in groupsList)
        {
            try
            {
                var rv = group.GetStudentsList().First(student => student.SelfId == id);
                return (ExtendedStudent)rv;
            }
            catch (InvalidOperationException)
            {
            }
        }

        return null;
    }

    public List<IStudent> FindStudents(IGroupName groupName)
    {
        IGroup? group = FindGroup(groupName);
        var foundStudents = new List<IStudent>();
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

    public List<IStudent> FindStudents(int courseNumber)
    {
        var suitableCourseGroups = FindGroups(courseNumber);
        if (suitableCourseGroups == null)
            return new List<IStudent>();

        List<IStudent> result = new List<IStudent>();
        foreach (var group in suitableCourseGroups)
        {
            result.AddRange(group.GetStudentsList());
        }

        return result;
    }

    public List<IGroup>? FindGroups(int courseNumber)
    {
        List<IGroup> result = new List<IGroup>();
        foreach (var group in groupsList)
        {
            if (group.CourseNumber == courseNumber)
            {
                result.Add(group);
            }
        }

        return result.Count == 0 ? null : result;
    }

    public void ChangeStudentGroup(ExtendedStudent student, ExtendedGroup newGroup)
    {
        student.ChangeGroup(newGroup);
    }

    public List<IStudent> GetAllStudentsWithExtraCourses(IGroup? basicGroup = null)
    {
        List<IStudent> matchingStudents = new List<IStudent>();

        foreach (var student in allStudentsList)
        {
            if (basicGroup == null || (student.StudentGroup == basicGroup &&
                    (((ExtendedStudent)student).IsOnlineExtraCourseAssigned() &&
                    (((ExtendedStudent)student).IsOfflineExtraCourseAssigned()))))
                matchingStudents.Add(student);
        }

        return matchingStudents;
    }

    public List<IStudent> GetAllStudentsWithoutExtraCourses(IGroup? group)
    {
        List<IStudent> matchingStudents = new List<IStudent>();

        foreach (var student in allStudentsList)
        {
            if (student.StudentGroup == group &&
                (!((ExtendedStudent)student).IsOnlineExtraCourseAssigned() ||
                (!((ExtendedStudent)student).IsOfflineExtraCourseAssigned())))
                matchingStudents.Add(student);
        }

        return matchingStudents;
    }

    public void DisplayAllStudentsInfo()
    {
        foreach (var student in allStudentsList)
        {
            Console.WriteLine($"Student {student.FullName} from {student.StudentGroup} " +
            $"(onlineExtra is {((ExtendedStudent)student).OnlineExtraCourseGroup})" +
            $"(offlineExtra is {((ExtendedStudent)student).OfflineExtraCourseGroup})");
        }
    }

    public List<ExtendedGroup> GetAllExtraCourseGroups()
    {
        List<ExtendedGroup> matchingGroups = new List<ExtendedGroup>();
        foreach (var course in extraCourseList)
        {
            foreach (var group in course.Groups)
            {
                matchingGroups.Add(group);
            }
        }

        return matchingGroups;
    }

    public ExtendedGroup? FindExtraGroup(ExtraGroupName groupName)
    {
        var allExtraGroups = this.GetAllExtraCourseGroups();

        foreach (var group in allExtraGroups)
        {
            if (group.GroupId.Equals(groupName))
                return group;
        }

        return null;
    }

    public List<IStudent> GetAllStudentsFromExtraGroup(ExtraGroupName groupName)
    {
        ExtendedGroup? foundGroup = FindExtraGroup(groupName);

        if (foundGroup == null)
            return new List<IStudent>();
        else
            return new List<IStudent>(foundGroup.GetStudentsList());
    }
}
