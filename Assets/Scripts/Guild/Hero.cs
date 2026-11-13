using UnityEngine;

public class Hero : IActionable
{
    [SerializeField] private HeroData _data;

    private HeroType _heroType;
    private string _name;
    private int _hp;
    private Room _currentRoom;

    public string Name => _name;
    public int CurrentRoomNumber => _currentRoom.RoomNumber;

    public Hero(HeroData data)
    {
        _data = data;
        _heroType = data.HeroType;
        _name = data.name;
        _hp = data.MaxHp;
    }

    public void SetRoom(Room room)
    {
        _currentRoom = room;
    }

    public void RemoveRoom(Room room)
    {
        _currentRoom = null;
    }
}
