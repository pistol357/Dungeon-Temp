public interface IActionable
{
    public int CurrentRoomNumber { get; }

    public void SetRoom(Room room);
}
