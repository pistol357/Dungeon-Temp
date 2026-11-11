using UnityEngine;

public class Monster
{
    [SerializeField] private MonsterData _data;

    private string _name;
    private int _hp;
    private Room _currentRoom;
    private State _state;

    public string Name => _name;
    public int CurrentRoomNumber => _currentRoom.RoomNumber;

    public void Move()
    {

    }

    public void Activate()
    {

    }

    public void SRAttack()
    {

    }

    public void LRAttack()
    {

    }

    public void Defense()
    {

    }

    public void SetRoom(Room room)
    {
        _currentRoom = room;
    }

    public void RemoveRoom(Room room)
    {
        _currentRoom = null;
    }

    public void Create(ActionType resource)
    {

    }

    public void SetData(MonsterData data)
    {
        _data = data;
        _name = data.name;
        _hp = data.MaxHp;
    }
}
