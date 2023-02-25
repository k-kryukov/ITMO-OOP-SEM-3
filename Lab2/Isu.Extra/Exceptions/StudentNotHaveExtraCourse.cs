namespace IsuExtra.Exceptions;

public class StudentNotHaveExtraCourse : Exception
{
    public StudentNotHaveExtraCourse(string studentFullName)
    : base($"Not able to delete {studentFullName} from his extra course - he is not assigned it") { }
}