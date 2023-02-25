namespace Banks.Entities;

public class Time
{
    private static uint _time = 0;

    public static uint CurTime { get { return _time; } }

    public static void MoveTimeForward(uint shift)
    {
        for (int i = 0; i < shift; ++i)
        {
            _time++;
            Notify();
        }
    }

    public static void Notify()
    {
        CentralBank.GetInstance().NotifyTimeToCalculatePercents();
    }
}