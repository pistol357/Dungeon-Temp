public interface IActionable
{
    public int CurrentRoomNumber { get; }
    public UnitState State { get; }

    public void SetRoom(Room room);

    public void Damage(int damage);

    public void Heal();
}
