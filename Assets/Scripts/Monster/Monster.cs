using UnityEngine;

public class Monster : IActionable
{
    [SerializeField] private MonsterData _data;

    private string _name;
    private int _hp;
    private Room _currentRoom;

    public string Name => _name;
    public int CurrentRoomNumber => _currentRoom.RoomNumber;

    public Monster(MonsterData data)
    {
        _data = data;
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
