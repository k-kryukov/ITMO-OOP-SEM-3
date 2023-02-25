namespace IsuExtra.Exceptions;

public class StudentNotFound : Exception
{
    public StudentNotFound(int id)
    : base($"Student with {id} is not found!") { }
}