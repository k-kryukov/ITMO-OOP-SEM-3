namespace IsuExtra;

using IsuExtra.Services;
using IsuExtra.Models;
using IsuExtra.Entities;
using System;

public class Program
{
    public static void PrintHint()
    {
        Console.WriteLine("\nInput an option you want to execute");
        Console.WriteLine(
            "______________________________\n" +
            "-1 - exit, 1 - add group " +
            "2 - add student to group\n3 - move student to another group, 4 - list all students\n" +
            "5 - list all groups, 6 - print this hint\n" +
            "______________________________\n");
    }

    public static void Main(string[] args)
    {
        var service = new IsuExtraService();

        var simpleGroup = service.AddGroup(new GroupName("M32011"));
        var student = service.AddStudent(simpleGroup, "k.kryukov");

        service.AddLesson(simpleGroup, new LessonTime(8, 20, "Monday"), new Teacher("A.Khastunov"), new Subject("Programming"), new Room("331"));
        service.AddLesson(simpleGroup, new LessonTime(8, 20, "Monday"), new Teacher("A.Khastunov"), new Subject("Programming"), new Room("331"));
    }
}
