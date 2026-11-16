using UnityEngine;

public class Monster : IActionable
{
    [SerializeField] private MonsterData _data;

    private string _name;
    private UnitState _state;
    private int _hp;
    private Room _currentRoom;

    public Sprite Icon => _data.Icon;
    public string Name => _name;
    public UnitState State => _state;
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

    public void Damage(int damage)
    {
        _hp -= damage;

        if (_hp <= 0)
        {
            Return();
            _hp = _data.MaxHp;
        }
    }

    public void Return()
    {
        _state = UnitState.None;
        ActionManager.Instance.Return(this);
    }
}
