namespace IsuExtra.Models;

public class Room
{
    private string _roomNumber;

    public Room(string roomNumber) { _roomNumber = roomNumber; }

    public string Number { get { return _roomNumber; } }

    public override string ToString() { return _roomNumber; }
}