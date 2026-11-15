using UnityEngine;

public class Hero : IActionable
{
    [SerializeField] private HeroData _data;

    private Guild _guild;
    private HeroType _heroType;
    private string _name;
    private UnitState _state;
    private int _hp;
    private Room _currentRoom;

    public HeroType HeroType => _heroType;
    public string Name => _name;
    public UnitState State => _state;
    public int CurrentRoomNumber => _currentRoom.RoomNumber;

    public Hero(HeroData data, Guild guild)
    {
        _data = data;
        _heroType = data.HeroType;
        _name = data.name;
        _hp = data.MaxHp;
        _guild = guild;
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
            Dead();
            _hp = _data.MaxHp;
        }
    }

    public void Cheer()
    {
        _state = UnitState.Active;
    }

    public void Exhaust()
    {
        _state = UnitState.Exhausted;
    }

    private void Dead()
    {
        _state = UnitState.None;
    }
}
