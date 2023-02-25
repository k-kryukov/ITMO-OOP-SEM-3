namespace Isu;

using Isu.Services;
using Isu.Models;
using Isu.Entities;
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
        IsuService service = new IsuService();

        Console.WriteLine("Welcome to ISU Menu");

        bool go = true;

        PrintHint();
        while (go)
        {
            int optionNumber = Convert.ToInt32(Console.ReadLine());
            if (optionNumber == -1)
            {
                break;
            }
            else if (optionNumber == 3)
            {
                Console.WriteLine("Input student id");
                int studentId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Input new group");
                var groupId = Console.ReadLine();
                if (groupId == null)
                {
                    Console.WriteLine("Unnable to read group id input");
                    continue;
                }

                Group? groupObj = service.FindGroup(new GroupName(groupId));
                Student? studentObj = service.FindStudent(studentId);
                if (groupObj == null)
                {
                    Console.WriteLine($"Group {groupId} doesn't exists in ISU!");
                }
                else if (studentObj == null)
                {
                    Console.WriteLine($"Student {studentId} doesn't exists in ISU!");
                }
                else
                {
                    service.ChangeStudentGroup(studentObj, groupObj);
                    Console.WriteLine($"Successfully moved student {studentId} to group {groupId}");
                }
            }
            else if (optionNumber == 2)
            {
                Console.WriteLine("Input student full name");
                var studentName = Console.ReadLine();
                if (studentName == null)
                {
                    Console.WriteLine("Unnable to read student name input");
                    continue;
                }

                Console.WriteLine("Input student group");
                var groupId = Console.ReadLine();
                if (groupId == null)
                {
                    Console.WriteLine("Unnable to read group id input");
                    continue;
                }

                var groupObj = service.FindGroup(new GroupName(groupId));
                if (groupObj == null)
                {
                    Console.WriteLine($"Group {groupId} doesn't exists in ISU!");
                }
                else
                {
                    service.AddStudent(groupObj, studentName);
                }
            }
            else if (optionNumber == 1)
            {
                Console.WriteLine("Input group ID");
                var groupId = Console.ReadLine();
                if (groupId == null)
                {
                    Console.WriteLine("Unnable to read group id input");
                    continue;
                }

                service.AddGroup(new GroupName(groupId));
            }
            else if (optionNumber == 4)
            {
                if (service.GetAllStudentsList().Any() != true)
                {
                    Console.WriteLine("There are no students registred in ISU!");
                    continue;
                }

                foreach (var student in service.GetAllStudentsList())
                {
                    Console.WriteLine(student);
                }
            }
            else if (optionNumber == 5)
            {
                if (service.GetAllGroupsList().Any() != true)
                {
                    Console.WriteLine("There are no groups registred in ISU!");
                    continue;
                }

                foreach (var group in service.GetAllGroupsList())
                {
                    Console.WriteLine(group);
                }
            }
            else if (optionNumber == 6)
            {
                PrintHint();
            }
            else
            {
                Console.WriteLine("Option is not available!");
            }
        }
    }
}