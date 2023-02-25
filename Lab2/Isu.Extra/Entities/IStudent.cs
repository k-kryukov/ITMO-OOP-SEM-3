namespace IsuExtra.Entities;

public interface IStudent
{
    string FullName { get; }
    int SelfId { get; }

    IGroup StudentGroup { get; }
}