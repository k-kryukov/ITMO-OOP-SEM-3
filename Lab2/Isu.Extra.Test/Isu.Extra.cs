using System;
using System.Collections.Generic;
using System.Linq;

using IsuExtra.Entities;
using IsuExtra.Exceptions;
using IsuExtra.Models;
using IsuExtra.Services;

using Xunit;

namespace IsuExtra.Test;

public class IsuExtraTest
{
    private IsuExtraService service;

    public IsuExtraTest()
    {
        service = new IsuExtraService();
    }

    [Fact]
    public void AddSimpleGroupWorksCorrectly()
    {
        service.AddGroup(new GroupName("M32011"));
        service.AddGroup(new GroupName("M31010"));
        service.AddGroup(new GroupName("K34012"));
        Assert.Throws<InvalidGroupName>(() => service.AddGroup(new GroupName("MUM")));
    }

    [Fact]
    public void AddStudentToSimeplGroupWorksCorrectly()
    {
        var group = service.AddGroup(new GroupName("M32011"));
        var newStudent = service.AddStudent(group, "k.kryukov");
        var studentsList = group.GetStudentsList();

        Assert.Equal(newStudent.StudentGroup, group);

        IStudent? foundStudent = null;
        foreach (var student in studentsList)
        {
            if (student.SelfId == newStudent.SelfId)
                foundStudent = student;
        }

        Assert.True(foundStudent != null);
    }

    [Fact]
    public void AddNewExtraCourseAndGroups()
    {
        var group = service.AddGroup(new GroupName("M32011"));
        var newStudent = service.AddStudent(group, "k.kryukov");

        var extraCourse = service.AddNewExtraCourse("Machine learning");
        service.AddNewExtraCourseGroup(extraCourse, new ExtraGroupName("KIB-3"));
        service.AddNewExtraCourseGroup(extraCourse, new ExtraGroupName("KIB-4"));
        Assert.Throws<InvalidGroupName>(() => service.AddNewExtraCourseGroup(extraCourse, new ExtraGroupName("BIBA-BOBA")));
    }

    [Fact]
    public void AddPersonToExtraCourseWithoutIntersection()
    {
        var group = service.AddGroup(new GroupName("M32011"));
        var newStudent = service.AddStudent(group, "k.kryukov");

        var extraCourse = service.AddNewExtraCourse("Machine learning");
        service.AddNewExtraCourseGroup(extraCourse, new ExtraGroupName("KIB-3"));

        service.AssignOnlineExtraCourseToStudent(extraCourse, newStudent);
    }

    [Fact]
    public void AddPersonToExtraCourseWithIntersection()
    {
        var group = service.AddGroup(new GroupName("M32011"));
        var newStudent = service.AddStudent(group, "k.kryukov");
        service.AddLesson(group, new LessonTime(8, 20, "Monday"), new Teacher("A.Khastunov"), new Subject("Programming"), new Room("331"));
        service.AddLesson(group, new LessonTime(10, 0, "Monday"), new Teacher("A.Khastunov"), new Subject("Programming"), new Room("331"));
        service.AddLesson(group, new LessonTime(11, 30, "Monday"), new Teacher("A.Khastunov"), new Subject("Programming"), new Room("331"));

        var extraCourse = service.AddNewExtraCourse("Machine learning");
        var extraGroup = service.AddNewExtraCourseGroup(extraCourse, new ExtraGroupName("KIB-3"));
        service.AddLesson(extraGroup, new LessonTime(10, 0, "Monday"), new Teacher("No.Name.Teacher"), new Subject("Boring subject"), new Room("331"));
        Assert.Throws<IntersectionWithMainSchedule>(() => service.AssignOnlineExtraCourseToStudent(extraCourse, newStudent));

        extraGroup = service.AddNewExtraCourseGroup(extraCourse, new ExtraGroupName("KIB-4"));
        service.AddLesson(extraGroup, new LessonTime(10, 0, "Tuesday"), new Teacher("No.Name.Teacher"), new Subject("Boring subject"), new Room("331"));
        service.AssignOnlineExtraCourseToStudent(extraCourse, newStudent);
    }

    [Fact]
    public void AddLessonWithWrongArgs_RaisedException()
    {
        var group = service.AddGroup(new GroupName("M32011"));
        var newStudent = service.AddStudent(group, "k.kryukov");

        Assert.Throws<WrongLessonTime>(() => service.AddLesson(group, new LessonTime(25, 20, "Monday"), new Teacher("A.Khastunov"), new Subject("Programming"), new Room("331")));
        Assert.Throws<WrongLessonTime>(() => service.AddLesson(group, new LessonTime(8, 20, "SomeDay"), new Teacher("A.Khastunov"), new Subject("Programming"), new Room("331")));
        Assert.Throws<WrongLessonTime>(() => service.AddLesson(group, new LessonTime(8, 228, "Monday"), new Teacher("A.Khastunov"), new Subject("Programming"), new Room("331")));
    }

    [Fact]
    public void AddLessonToSameTimeslot()
    {
        var group = service.AddGroup(new GroupName("M32011"));
        var newStudent = service.AddStudent(group, "k.kryukov");

        service.AddLesson(group, new LessonTime(10, 0, "Tuesday"), new Teacher("No.Name.Teacher"), new Subject("Boring subject"), new Room("331"));
        Assert.Throws<TimeslotAlreadyOccupated>(() => service.AddLesson(group, new LessonTime(10, 0, "Tuesday"), new Teacher("Second.NoName.Teacher"), new Subject("Another Boring subject"), new Room("302")));
    }
}