using Isu.Exceptions;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Test;

public class IsuServiceTest
{
    private IsuService service;

    public IsuServiceTest()
    {
        service = new IsuService();
    }

    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        // arrange
        string groupName = "M31011";
        string studentName = "Konstantin Kryukov";

        // act
        var testGroup = service.AddGroup(new GroupName(groupName));
        var newStudent = service.AddStudent(testGroup, studentName);

        // assert
        Assert.True(newStudent.StudentGroup == testGroup);

        // check if group has student
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        string groupName = "M31011";
        var testGroup = service.AddGroup(new GroupName(groupName));

        for (int i = 0; i < testGroup.MaxStudentsNumberInGroup; ++i)
        {
            service.AddStudent(testGroup, $"Student {i}");
        }

        Assert.Throws<MaxStudentsNumberPerGroupReached>(() => service.AddStudent(testGroup, "Odd Student"));
    }

    [Theory]
    [InlineData("aboba")]
    [InlineData("M3")]
    [InlineData("abobus")]
    [InlineData("M30011")]
    [InlineData("M35011")]
    public void CreateGroupWithInvalidName_ThrowException(string groupName)
    {
        Assert.Throws<InvalidGroupName>(() => service.AddGroup(new GroupName(groupName)));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        // arrange
        string initialGroupName = "M32011";
        string newGroupName = "M32001";
        string studentName = "Konstantin Kryukov";

        var initialGroup = service.AddGroup(new GroupName(initialGroupName));
        var newGroup = service.AddGroup(new GroupName(newGroupName));

        var student = service.AddStudent(initialGroup, studentName);

        service.ChangeStudentGroup(student, newGroup);

        Assert.True(student.StudentGroup == newGroup);
    }
}