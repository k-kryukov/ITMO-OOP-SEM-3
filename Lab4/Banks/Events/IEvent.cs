using Banks.Entities;
namespace Banks.Events;

public interface IEvent
{
    Guid GetEventId();
    void LogUpdate();
    IEvent FillWithInfo(IBank bank, IClient client);
}