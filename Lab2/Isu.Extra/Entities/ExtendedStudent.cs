namespace IsuExtra.Entities;

using IsuExtra.Models;
using IsuExtra.Exceptions;

public class ExtendedStudent : Student, IStudent
{
    private ExtendedGroup? onlineExtraCourseGroup;
    private ExtendedGroup? offlineExtraCourseGroup;

    public ExtendedStudent(string fullName, IGroup group, int id)
    : base(fullName, group, id)
    {
        onlineExtraCourseGroup = null;
        offlineExtraCourseGroup = null;
    }

    public ExtendedGroup? OnlineExtraCourseGroup { get { return onlineExtraCourseGroup; } }
    public ExtendedGroup? OfflineExtraCourseGroup { get { return offlineExtraCourseGroup; } }

    public void ChangeOnlineExtraCourseGroup(ExtendedGroup newGroup) { onlineExtraCourseGroup = newGroup; }
    public void ChangeOfflineExtraCourseGroup(ExtendedGroup newGroup) { offlineExtraCourseGroup = newGroup; }

    public bool IsOnlineExtraCourseAssigned() { return onlineExtraCourseGroup != null; }
    public bool IsOfflineExtraCourseAssigned() { return offlineExtraCourseGroup != null; }

    public ExtendedGroup RemoveFromOfflineExtraCourse()
    {
        if (offlineExtraCourseGroup == null)
            throw new StudentNotHaveExtraCourse(this.FullName);

        var exExtraCourseGroup = offlineExtraCourseGroup;
        offlineExtraCourseGroup = null;

        return exExtraCourseGroup;
    }

    public ExtendedGroup RemoveFromOnlineExtraCourse()
    {
        if (onlineExtraCourseGroup == null)
            throw new StudentNotHaveExtraCourse(this.FullName);

        var exExtraCourseGroup = onlineExtraCourseGroup;
        onlineExtraCourseGroup = null;

        return exExtraCourseGroup;
    }
}